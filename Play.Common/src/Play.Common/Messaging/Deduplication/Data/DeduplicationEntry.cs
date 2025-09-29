namespace Play.Common.Messaging.Deduplication.Data;

public class DeduplicationEntry
{
    public string MessageId { get; set; }
    public DateTimeOffset ProcessedAt { get; set; }
}