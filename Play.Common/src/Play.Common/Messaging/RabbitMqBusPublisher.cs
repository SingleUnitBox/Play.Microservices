using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
using Play.Common.Serialization;
using RabbitMQ.Client;

namespace Play.Common.Messaging;

internal sealed class RabbitMqBusPublisher(
    ChannelFactory channelFactory,
    ISerializer serializer,
    MessagePropertiesAccessor messagePropertiesAccessor) : IBusPublisher
{
    public async Task Publish<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        string routingKey = "",
        ICorrelationContext correlationContext = null,
        IDictionary<string, object?> headers = default)
        where TMessage : class
    {
        var channel = channelFactory.CreateForProducer();
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
        
        await Task.CompletedTask;
    }

    private IBasicProperties CreateMessageProperties<TMessage>(
        IModel channel,
        string? messageId = default,
        ICorrelationContext correlationContext = default,
        IDictionary<string, object>? headers = default)
    {
        var messageProperties = messagePropertiesAccessor.Get();
        
        var basicProperties = channel.CreateBasicProperties();
        basicProperties.MessageId = messageId ?? Guid.NewGuid().ToString();
        basicProperties.Type = typeof(TMessage).Name;
        basicProperties.CorrelationId = string.IsNullOrWhiteSpace(correlationContext?.CorrelationId.ToString())
            ? Guid.NewGuid().ToString()
            : correlationContext.CorrelationId.ToString();
        basicProperties.Headers = new Dictionary<string, object>();
        // {
        //     { "UserId", correlationContext?.UserId.ToString() ?? Guid.Empty.ToString() }
        // };

        var headersToAdd = headers
            ?? messageProperties?.Headers
            ?? new Dictionary<string, object>();

        foreach (var header in headersToAdd)
        {
            basicProperties.Headers.Add(header.Key, header.Value.ToString());
        }
        
        return basicProperties;
    }
}