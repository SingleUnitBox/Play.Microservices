using System.Text;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.RabbitMq.Connection;
using Play.Common.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Common.RabbitMq.Consumers;

internal sealed class EventConsumer(
    ChannelFactory channelFactory,
    IEventDispatcher eventDispatcher,
    ISerializer serializer,
    ILogger<EventConsumer> logger,
    IServiceProvider serviceProvider) : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = default) where TEvent : class, IEvent
    {
        var channel = channelFactory.CreateForConsumer();

        if (queueName is null)
        {
            queueName = typeof(TEvent).GetQueueName();
        }
        // channel.QueueDeclare(queueName, true, false, false);
        //
        // var exchangeName = typeof(TEvent).GetExchangeName();
        // var routingKey = typeof(TEvent).GetRoutingKey();
        // channel.QueueBind(queueName, exchangeName, routingKey);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            // var correlationId = ea.BasicProperties?.CorrelationId ?? Guid.Empty.ToString();
            // var userIdString = string.Empty;
            // if (ea.BasicProperties?.Headers?.TryGetValue("UserId", out var userIdHeader) == true &&
            //     userIdHeader is byte[] userIdBytes)
            // {
            //     userIdString = Encoding.UTF8.GetString(userIdBytes);
            // }
            
            // var userId = Guid.TryParse(userIdString, out var userIdGuid)
            //     ? userIdGuid
            //     : Guid.Empty;
            
            // var correlationContextAccessor = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
            // correlationContextAccessor.CorrelationContext = 
            //     new CorrelationContext.CorrelationContext(Guid.Parse(correlationId), userId);
            
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = serializer.Deserialize<TEvent>(message);

                await eventDispatcher.HandleAsync(@event!);

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                channel.BasicAck(ea.DeliveryTag, false);
            }
        };

        channel.BasicConsume(queueName, false, consumer);
    }
}