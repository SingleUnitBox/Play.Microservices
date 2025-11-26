using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Play.Common.Messaging.Outbox.Data;

internal sealed class OutboxLocalCache(MessagePropertiesAccessor messagePropertiesAccessor,
    ILogger<OutboxLocalCache> logger)
{
    private readonly IDictionary<string, OutboxPendingMessages> _cache = new ConcurrentDictionary<string, OutboxPendingMessages>();

    public IReadOnlyList<OutboxMessage> GetForPublish()
    {
        var messageId = messagePropertiesAccessor.Get()?.MessageId;
        if (messageId is null)
        {
            return new List<OutboxMessage>();
        }
        
        var hasPendingMessages = _cache.TryGetValue(messageId, out var pendingMessages);
        _cache.Remove(messageId);
        
        logger.LogInformation($"Outbox local cache for messageId '{messageId}' returns '{pendingMessages!.Messages.Count}' pending messages to be published.");
        return hasPendingMessages 
            ? pendingMessages.Messages
            : new List<OutboxMessage>();
    }

    public void Initialize()
    {
        var messageId = messagePropertiesAccessor.Get()?.MessageId;
        if (messageId is null || _cache.ContainsKey(messageId))
        {
            return;
        }
        
        _cache.Add(messageId, new OutboxPendingMessages());
        logger.LogInformation($"Outbox local cache initialized for messageId '{messageId}'.");
    }

    public void Add(OutboxMessage outboxMessage)
    {
        var messageId = messagePropertiesAccessor.Get()?.MessageId;
        if (messageId is null)
        {
            return;
        }

        _cache.TryGetValue(messageId, out var pendingMessages);
        pendingMessages?.Messages.Add(outboxMessage);
    }

    private class OutboxPendingMessages
    {
        public List<OutboxMessage> Messages { get; } = new();
    }
}