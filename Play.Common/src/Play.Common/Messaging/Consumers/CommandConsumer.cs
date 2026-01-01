using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
using Play.Common.Messaging.Message;
using Play.Common.Messaging.Resiliency;
using Play.Common.Messaging.Topology;
using Play.Common.Observability.Tracing;
using Play.Common.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.Messaging.Consumers;

internal sealed class CommandConsumer(
    ChannelFactory channelFactory,
    ICommandDispatcher commandDispatcher,
    ISerializer serializer,
    ILogger<CommandConsumer> logger,
    IServiceProvider serviceProvider,
    IExceptionToMessageMapper exceptionToMessageMapper,
    MessagePropertiesAccessor messagePropertiesAccessor,
    TopologyReadinessAccessor topologyReadinessAccessor,
    ReliableConsuming reliableConsuming)
    : ICommandConsumer
{
    public async Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand
    {
        var channel = channelFactory.CreateForConsumer();
        var queueName = typeof(TCommand).GetQueueName();
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            // correlationContext ?? should be here
            SetCorrelationContext(ea.BasicProperties);
            SetMessageProperties(ea.BasicProperties);
            
            using var activity = CreateMessagingConsumeActivity(ea.BasicProperties);

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var command = serializer.Deserialize<TCommand>(message);
            
            try
            { 
                await commandDispatcher.DispatchAsync(command);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, $"Error while processing command {typeof(TCommand).Name}");
                await OnCommandDispatcherFailure<TCommand>(ea, channel);
                return;
            }
            
            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        
        await topologyReadinessAccessor.EnsureTopologyReadiness(stoppingToken);
        channel.BasicConsume(queueName, false, consumer);
    }

    public Task ConsumeNonGenericCommand(Func<MessageData, Task> handleRawPayload,
        string queue,
        CancellationToken stoppingToken = default)
    {
        var channel = channelFactory.CreateForConsumer();
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var messageData = CreateMessageData(ea);
                await handleRawPayload(messageData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            channel.BasicAck(ea.DeliveryTag, multiple: false);
        };
        
        channel.BasicConsume(queue, false, consumer);
        return Task.CompletedTask;
    }

    private async Task OnCommandDispatcherFailure<TCommand>(BasicDeliverEventArgs ea, IModel channel)
    {
        var messageId = GetMessageId(ea.BasicProperties);
        reliableConsuming.OnConsumeFailed(messageId);
        
        if (reliableConsuming.CanBrokerRetry(messageId))
        {
            logger.LogWarning($"Command consume failed for message with id '{messageId}. Retrying via broker...'");
            channel.BasicNack(ea.DeliveryTag, false, requeue: true);
        }
        else
        {
            logger.LogError($"Broker retries limit exhausted for message with id '{messageId}'. Rejecting message...");
            channel.BasicReject(ea.DeliveryTag, requeue: false);
        }
    }

    private MessageData CreateMessageData(BasicDeliverEventArgs ea)
    {
        var messageId = GetMessageId(ea.BasicProperties);
        var messageType = ea.BasicProperties.Type;
        var payload = ea.Body.ToArray();
        
        return new MessageData(messageId, payload, messageType);
    }

    private static Guid GetMessageId(IBasicProperties properties)
    {
        var messageId = properties.MessageId;
        return Guid.Parse(messageId);
    }

    private void SetCorrelationContext(IBasicProperties basicProperties)
    {
        var correlationId = basicProperties.CorrelationId ?? Guid.Empty.ToString();
        var userIdString = string.Empty;
        if (basicProperties.Headers?.TryGetValue("UserId", out var userIdHeader) == true &&
            userIdHeader is byte[] userIdBytes)
        {
            userIdString = Encoding.UTF8.GetString(userIdBytes);
        }
            
        var userId = Guid.TryParse(userIdString, out var userIdGuid)
            ? userIdGuid
            : Guid.Empty;
        
        var correlationContextAccessor = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
        correlationContextAccessor.CorrelationContext = 
            new CorrelationContext.CorrelationContext(Guid.Parse(correlationId), userId);
    }

    private void SetMessageProperties(IBasicProperties basicProperties)
    {
        var messageId = basicProperties.MessageId;
        var headers = basicProperties.Headers?
            .Select(h => (h.Key, (object)Encoding.UTF8.GetString((byte[])h.Value))).ToDictionary();
        var messageType = basicProperties.Type;
        
        var messageProperties = new MessageProperties(messageId, headers, messageType);
        messagePropertiesAccessor.Set(messageProperties);
    }

    private Activity? CreateMessagingConsumeActivity(IBasicProperties basicProperties)
    {
        var isHeaderPresent = basicProperties.Headers?.ContainsKey(MessagingObservabilityHeaders.TraceParent) ?? false;
        if (isHeaderPresent is false)
        {
            return Activity.Current;
        }

        basicProperties.Headers.TryGetValue(MessagingObservabilityHeaders.TraceParent, out var traceIdBytes);
        var traceId = Encoding.UTF8.GetString((byte[])traceIdBytes);

        var parentContext = ActivityContext.Parse(traceId, default);
        var activitySource = new ActivitySource(MessagingActivitySource.MessagingConsumeSourceName);
        var activity = activitySource.StartActivity(
            $"Command consuming: {messagePropertiesAccessor.Get()?.MessageType}",
            ActivityKind.Consumer,
            parentContext: parentContext,
            links: [new ActivityLink(parentContext)]);
        
        return activity;
    }
}