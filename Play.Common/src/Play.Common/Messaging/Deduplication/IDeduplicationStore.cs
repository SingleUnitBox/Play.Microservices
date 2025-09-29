namespace Play.Common.Messaging.Deduplication;

public interface IDeduplicationStore
{
    Task AddEntryAsync(string messageId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string messageId, CancellationToken cancellationToken = default);
}