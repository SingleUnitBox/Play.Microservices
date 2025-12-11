using Microsoft.Extensions.Logging;
using Play.Common.Abs.Messaging;
using Play.Common.Abs.RabbitMq;

namespace Play.Common.Messaging.Outbox;

internal sealed class OutboxMessagePublisher(IMessageOutbox messageOutbox, MessagePropertiesAccessor messagePropertiesAccessor,
    ILogger<OutboxMessagePublisher> logger) : IBusPublisher
{
    public async Task PublishAsync<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        string routingKey = "",
        ICorrelationContext correlationContext = null,
        IDictionary<string, object> headers = default,
        CancellationToken cancellationToken = default)
        where TMessage : class
    {
        logger.LogError("AddAsync called. ThreadId={ThreadId}. Publisher stack:\n{Stack}",
            Environment.CurrentManagedThreadId,
            Environment.StackTrace);
        
        var messageProperties = messagePropertiesAccessor.Get();
        var messageIdSafe = messageProperties?.MessageId ?? messageId;

        await messageOutbox.AddAsync(
            message,
            messageIdSafe,
            exchangeName,
            routingKey,
            messageProperties?.Headers ?? headers,
            cancellationToken);
        logger.LogInformation($"Message '{message.GetType().Name}' has been saved to Outbox.");
    }
}
