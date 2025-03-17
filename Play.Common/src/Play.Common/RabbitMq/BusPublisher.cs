using System.Text;
using Newtonsoft.Json;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Connection;
using Play.Common.Serialization;
using RabbitMQ.Client;

namespace Play.Common.RabbitMq;

internal sealed class BusPublisher(
    ChannelFactory channelFactory,
    ISerializer serializer) : IBusPublisher
{
    public async Task Publish<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        ICorrelationContext correlationContext = null,
        IDictionary<string, object?> headers = default)
        where TMessage : class
    {
        var channel = channelFactory.CreateForProducer();

        //create_item_exchange
        if (exchangeName is null)
        {
            exchangeName = message.GetExchangeName();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false);
        }

        //create_item_queue
        // var queueName = message.GetQueueName();
        // channel.QueueDeclare(queueName, true, false, false);
        
        var routingKey = message.GetRoutingKey();
        // channel.QueueBind(queueName, exchangeName, routingKey);

        var body = serializer.Serialize(message);
        
        //properties
        var basicProperties = CreateMessageProperties<TMessage>(
            channel: channel,
            messageId: messageId,
            correlationContext: correlationContext,
            headers: headers);
        
        channel.BasicPublish(
            exchangeName,
            routingKey,
            false,
            basicProperties,
            body);
    }

    private IBasicProperties CreateMessageProperties<TMessage>(
        IModel channel,
        string messageId = default,
        ICorrelationContext correlationContext = default,
        IDictionary<string, object?> headers = default)
    {
        var basicProperties = channel.CreateBasicProperties();
        basicProperties.MessageId = messageId ?? Guid.NewGuid().ToString();
        basicProperties.Type = typeof(TMessage).Name;
        basicProperties.CorrelationId = string.IsNullOrWhiteSpace(correlationContext?.CorrelationId.ToString())
            ? Guid.NewGuid().ToString()
            : correlationContext.CorrelationId.ToString();
        basicProperties.Headers = new Dictionary<string, object>
        {
            { "UserId", correlationContext?.UserId.ToString() ?? Guid.Empty.ToString() }
        };
        
        return basicProperties;
    }
}