using Play.Common.Abs.Messaging;

namespace Play.Common.Messaging.Outbox;

public interface IMessageOutbox
{
    Task AddAsync<TMessage>(
        TMessage message,
        string messageId,
        string? exchange = default,
        string? routingKey = default,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default)
        where TMessage : class;
    
    Task<IReadOnlyList<OutboxMessage>> GetUnsentAsync(
        int batchSize = default,
        CancellationToken cancellationToken = default);
    
    Task MarkAsProcessedAsync(
        OutboxMessage outboxMessage,
        CancellationToken cancellationToken = default);
}