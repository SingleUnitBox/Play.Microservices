using System.Text;
using System.Text.Json;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public class  BusPublisher(IConnection connection) : IBusPublisher
{
    public async Task Publish<TMessage>(TMessage message) where TMessage : class
    {
        using var channel = await connection.CreateChannelAsync();

        //create_item_exchange
        var exchangeName = message.GetExchangeName();
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, true, false);

        //create_item_queue
        var queueName = message.GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);

        //create_item
        var routingKey = message.GetRoutingKey();
        // await channel.QueueBindAsync(queueName, exchangeName, routingKey);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchangeName, routingKey, body: body);
    }
}