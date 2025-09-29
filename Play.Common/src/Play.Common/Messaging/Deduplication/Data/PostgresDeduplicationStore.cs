using Microsoft.EntityFrameworkCore;

namespace Play.Common.Messaging.Deduplication.Data;

internal class PostgresDeduplicationStore(DeduplicationDbContext dbContext) : IDeduplicationStore
{
    public async Task AddEntryAsync(string messageId, CancellationToken cancellationToken = default)
    {
        var entry = new DeduplicationEntry { MessageId = messageId, ProcessedAt = DateTimeOffset.UtcNow };
        await dbContext.DeduplicationEntries.AddAsync(entry, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(string messageId, CancellationToken cancellationToken = default)
    {
        return dbContext.DeduplicationEntries.AnyAsync(e => e.MessageId == messageId, cancellationToken);
    }
}