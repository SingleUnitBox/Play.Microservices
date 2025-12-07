using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
using Play.Common.Messaging.Message;
using Play.Common.Messaging.Topology;
using Play.Common.Observability.Tracing;
using Play.Common.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.Messaging.Consumers;

internal sealed class EventConsumer(
    ChannelFactory channelFactory,
    IEventDispatcher eventDispatcher,
    ISerializer serializer,
    ILogger<EventConsumer> logger,
    IServiceProvider serviceProvider,
    IExceptionToMessageMapper exceptionToMessageMapper,
    IBusPublisher busPublisher,
    TopologyReadinessAccessor topologyReadinessAccessor,
    MessagePropertiesAccessor messagePropertiesAccessor)
    : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = default) where TEvent : class, IEvent
    {
        var channel = channelFactory.CreateForConsumer();
        channel.BasicQos(0, 1, false);
        if (queueName is null)
        {
            queueName = typeof(TEvent).GetQueueName();
        }
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            SetCorrelationContext(ea.BasicProperties);
            SetMessageProperties(ea.BasicProperties);

            using var activity = CreateMessagingConsumeActivity(ea.BasicProperties);
            
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = serializer.Deserialize<TEvent>(message);
            
            try
            {
                await eventDispatcher.HandleAsync(@event);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                var rejectedEvent = exceptionToMessageMapper.Map(e, @event);
                channel.BasicAck(ea.DeliveryTag, false);
                await busPublisher.PublishAsync(rejectedEvent);
            }

        };

        await EnsureTopologyReadiness(stoppingToken);
        channel.BasicConsume(queueName, false, consumer);
    }

    public Task ConsumeNonGenericEvent(Func<MessageData, Task> handleRawPayload, string queue, CancellationToken cancellationToken = default)
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
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            
            channel.BasicAck(ea.DeliveryTag, false);
        };
        
        channel.BasicConsume(queue, false, consumer);
        return Task.CompletedTask;
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

    private async Task EnsureTopologyReadiness(CancellationToken stoppingToken)
    {
        while (topologyReadinessAccessor.TopologyProvisioned is false)
        {
            logger.LogInformation("Waiting for topology to be provisioned...");
            await Task.Delay(1_000, stoppingToken);
        }
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
        var isHeaderPresent =
            basicProperties.Headers?.ContainsKey(MessagingObservabilityHeaders.TraceParent) ?? false;
        if (isHeaderPresent is false)
        {
            return Activity.Current;
        }

        basicProperties.Headers.TryGetValue(MessagingObservabilityHeaders.TraceParent, out var traceIdBytes);
        var traceId = Encoding.UTF8.GetString((byte[])traceIdBytes);

        var parentContext = ActivityContext.Parse(traceId, default);
        var activitySource = new ActivitySource(MessagingActivitySource.MessagingConsumeSourceName);
        var activity = activitySource.StartActivity(
            $"Event consuming: {messagePropertiesAccessor.Get()?.MessageType}",
            ActivityKind.Consumer,
            parentContext: parentContext,
            links: [new ActivityLink(parentContext)]);
        
        return activity;
    }
}