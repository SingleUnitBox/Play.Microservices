using Microsoft.Extensions.Logging;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Connection;
using Play.Common.Messaging.Resiliency;
using Play.Common.Serialization;
using RabbitMQ.Client;

namespace Play.Common.Messaging;

internal sealed class RabbitMqBusPublisher(
    ChannelFactory channelFactory,
    ISerializer serializer,
    MessagePropertiesAccessor messagePropertiesAccessor,
    ReliablePublishing reliablePublishing,
    ILogger<RabbitMqBusPublisher> logger) : IBusPublisher
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
            headers: headers,
            messageType: message.GetType());

        ConfigureReliablePublishing<TMessage>(channel, basicProperties.MessageId);
        
        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: reliablePublishing.ShouldPublishAsMandatory(),
            basicProperties,
            body);

        EnsureReliablePublishing(channel);
        
        await Task.CompletedTask;
    }

    private IBasicProperties CreateMessageProperties<TMessage>(
        IModel channel,
        string? messageId = default,
        ICorrelationContext correlationContext = default,
        IDictionary<string, object>? headers = default,        
        Type? messageType = default)
    {
        var messageProperties = messagePropertiesAccessor.Get();
        
        var basicProperties = channel.CreateBasicProperties();
        basicProperties.MessageId = messageId ?? Guid.NewGuid().ToString();
        // basicProperties.Type = typeof(TMessage).Name;
        basicProperties.Type = messageType.Name;
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

    private void ConfigureReliablePublishing<TMessage>(IModel channel, string messageId)
    {
        if (reliablePublishing.UsePublisherConfirms)
        {
            channel.ConfirmSelect();
            channel.BasicNacks += (sender, args) =>
            {
                logger.LogWarning(
                    $"Message '{typeof(TMessage).Name}' with id '{messageId}' was not accepted by broker.");
            };
        }

        if (reliablePublishing.ShouldPublishAsMandatory())
        {
            channel.BasicReturn += (sender, args) =>
            {
                logger.LogWarning($"Message '{typeof(TMessage)}' with id '{messageId}' was not routed properly to any consumer.");
            };
        }
    }

    private void EnsureReliablePublishing(IModel channel)
    {
        if (reliablePublishing.UsePublisherConfirms)
        {
            try
            {
                channel.WaitForConfirmsOrDie();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

}