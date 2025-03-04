using System.Text;
using Newtonsoft.Json;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Connection;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

internal sealed class BusPublisher(ChannelFactory channelFactory) : IBusPublisher
{
    public async Task Publish<TMessage>(
        TMessage message,
        string messageId = null,
        ICorrelationContext correlationContext = null)
        where TMessage : class
    {
        var channel = channelFactory.CreateForProducer();

        //create_item_exchange
        var exchangeName = message.GetExchangeName();
        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);

        //create_item_queue
        var queueName = message.GetQueueName();
        channel.QueueDeclare(queueName, true, false, false);
        
        var routingKey = message.GetRoutingKey();
        channel.QueueBind(queueName, exchangeName, routingKey);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        
        //properties
        var basicProperties = channel.CreateBasicProperties();
        basicProperties.CorrelationId = string.IsNullOrWhiteSpace(correlationContext?.CorrelationId.ToString())
            ? Guid.NewGuid().ToString()
            : correlationContext.CorrelationId.ToString();
        basicProperties.Headers = new Dictionary<string, object?>
        {
            { "UserId", correlationContext?.UserId.ToString() ?? Guid.Empty.ToString() }
        };
        
        channel.BasicPublish(
            exchangeName,
            routingKey,
            false,
            basicProperties,
            body);
    }
}