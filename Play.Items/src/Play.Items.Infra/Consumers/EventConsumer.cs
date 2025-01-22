using System.Text;
using System.Text.Json;
using Play.Common.Abs.Events;
using Play.Common.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Play.Items.Infra.Consumers;

public class EventConsumer(
    IConnection connection,
    IEventDispatcher eventDispatcher) : IEventConsumer
{
    public async Task ConsumeEvent<TEvent>() where TEvent : class, IEvent
    {
        using var channel = await connection.CreateChannelAsync();

        var queueName = typeof(TEvent).GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<TEvent>(message);

                await eventDispatcher.HandleAsync(@event);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception e)
            {
                await channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await channel.BasicConsumeAsync(queueName, false, consumer);
        await Task.Delay(Timeout.Infinite);
    }
}