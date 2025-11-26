using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Messaging;
using Play.Common.Serialization;

namespace Play.Common.Messaging.Outbox.Data;

internal sealed class PostgresMessageOutbox(
    OutboxDbContext dbContext,
    OutboxLocalCache cache,
    ISerializer serializer,
    ILogger<PostgresMessageOutbox> logger) : IMessageOutbox
{
    public async Task AddAsync<TMessage>(
        TMessage message,
        string messageId,
        string? exchange = default,
        string? routingKey = default,
        IDictionary<string, object>? headers = default,
        CancellationToken cancellationToken = default)
        where TMessage : class
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            Destination = exchange,
            RoutingKey = routingKey,
            Headers = headers ?? new Dictionary<string, object>(),
            Message = message,
            SerializedMessage = Encoding.UTF8.GetString(serializer.Serialize(message)),
            MessageType = message.GetType().AssemblyQualifiedName,
            StoredAt = DateTimeOffset.UtcNow
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        cache.Add(outboxMessage);
        logger.LogInformation($"Message '{outboxMessage.MessageType}' with id '{outboxMessage.Id}' has been added.");
    }

    public async Task<IReadOnlyList<OutboxMessage>> GetUnsentAsync(int batchSize = default, CancellationToken cancellationToken = default)
    {
        var sqlQuery =
            $"SELECT * FROM outbox.\"OutboxMessages\" WHERE \"ProcessedAt\" IS NULL ORDER BY \"StoredAt\" DESC {(batchSize >0? $"LIMIT {batchSize}" : "")} FOR UPDATE SKIP LOCKED";
        
        return await dbContext.OutboxMessages
            .FromSqlRaw(sqlQuery)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken = default)
    {
        outboxMessage.MarkAsProcessed();
        
        dbContext.OutboxMessages.Update(outboxMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}