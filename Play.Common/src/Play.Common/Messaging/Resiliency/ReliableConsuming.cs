using System.Collections.Concurrent;
using Play.Common.Settings;

namespace Play.Common.Messaging.Resiliency;

public class ReliableConsuming
{
    private readonly ConcurrentDictionary<Guid, int> _messageIdAttempsMade = new();
    private readonly bool _brokerRetriesEnabled;
    private readonly int _brokerRetriesLimit;
    private readonly int _consumerRetriesLimit;
    
    public ReliableConsuming(ResiliencySettings  resiliencySettings)
    {
        _brokerRetriesEnabled = resiliencySettings.Consumer.BrokerRetriesEnabled;
        _brokerRetriesLimit = resiliencySettings.Consumer.BrokerRetriesLimit;
        _consumerRetriesLimit = resiliencySettings.Consumer.ConsumerRetriesLimit;
    }
    
    public bool CanBrokerRetry(Guid messageId)
    {
        if (_brokerRetriesEnabled is false)
        {
            return true;
        }

        if (!_messageIdAttempsMade.TryGetValue(messageId, out int consumeAttempts))
        {
            _messageIdAttempsMade.TryAdd(messageId, 0);
            consumeAttempts = 0;
        }

        return consumeAttempts < _brokerRetriesLimit + 1;
    }

    public void OnConsumeFailed(Guid messageId)
    {
        if (_brokerRetriesEnabled is false)
        {
            return;
        }

        if (!_messageIdAttempsMade.TryGetValue(messageId, out int consumeAttempts))
        {
            _messageIdAttempsMade.TryAdd(messageId, 0);
            consumeAttempts = 0;
        }
        
        _messageIdAttempsMade.TryUpdate(messageId, consumeAttempts + 1, consumeAttempts);
    }
}