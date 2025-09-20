using System.Text;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.RabbitMq;
using Play.Common.RabbitMq.Connection;
using Play.Common.RabbitMq.Consumers;
using Play.Common.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Inventory.Infra.Events;

public class InventoryEventConsumer(
    ChannelFactory channelFactory,
    IEventDispatcher eventDispatcher,
    ISerializer serializer,
    ILogger<InventoryEventConsumer> logger,
    IServiceProvider serviceProvider) : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = new CancellationToken()) where TEvent : class, IEvent
    {
        var channel = channelFactory.CreateForConsumer();
        
        var queue = typeof(TEvent).GetQueueName();
        channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = serializer.Deserialize<TEvent>(message);

                await eventDispatcher.HandleAsync(@event);

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
