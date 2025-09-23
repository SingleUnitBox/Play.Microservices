using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
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
    IBusPublisher busPublisher)
    : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = default) where TEvent : class, IEvent
    {
        var channel = channelFactory.CreateForConsumer();
        if (queueName is null)
        {
            queueName = typeof(TEvent).GetQueueName();
        }
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            SetCorrelationContext(ea.BasicProperties);

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var command = serializer.Deserialize<TEvent>(message);
            
            try
            {
                await eventDispatcher.HandleAsync(command);
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