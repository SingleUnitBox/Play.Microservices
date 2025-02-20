using System.Text;
using Newtonsoft.Json;
using Play.Common.Abs.RabbitMq;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

public class  BusPublisher(IConnection connection) : IBusPublisher
{
    public async Task Publish<TMessage>(
        TMessage message,
        string messageId = null,
        ICorrelationContext correlationContext = null)
        where TMessage : class
    {
        using var channel = await connection.CreateChannelAsync();

        //create_item_exchange
        var exchangeName = message.GetExchangeName();
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, true, false);

        //create_item_queue
        var queueName = message.GetQueueName();
        await channel.QueueDeclareAsync(queueName, true, false, false);
        
        var routingKey = message.GetRoutingKey();
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        //properties
        var basicProperties = new BasicProperties();
        basicProperties.CorrelationId = string.IsNullOrWhiteSpace(correlationContext?.CorrelationId.ToString())
            ? Guid.NewGuid().ToString()
            : correlationContext.CorrelationId.ToString();
        basicProperties.Headers = new Dictionary<string, object?>
        {
            { "UserId", correlationContext?.UserId.ToString() ?? Guid.Empty.ToString() }
        };
        

        await channel.BasicPublishAsync(
            exchangeName,
            routingKey,
            false,
            basicProperties,
            body);
    }
}