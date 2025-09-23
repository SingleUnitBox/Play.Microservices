using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
using Play.Common.Messaging.Message;
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
    IBusPublisher busPublisher)
    : ICommandConsumer
{
    public async Task ConsumeCommand<TCommand>(CancellationToken stoppingToken) where TCommand : class, ICommand
    {
        var channel = channelFactory.CreateForConsumer();
        var queueName = typeof(TCommand).GetQueueName();
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            SetCorrelationContext(ea.BasicProperties);

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var command = serializer.Deserialize<TCommand>(message);
            
            try
            {
                await commandDispatcher.DispatchAsync(command);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception e)
            {
                var rejectedEvent = exceptionToMessageMapper.Map(e, command);
                channel.BasicAck(ea.DeliveryTag, false);
                await busPublisher.Publish(rejectedEvent);
            }

        };
        
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
}