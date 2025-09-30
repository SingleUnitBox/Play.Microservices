namespace Play.Common.Messaging;

public sealed class MessagePropertiesAccessor
{
    private readonly AsyncLocal<MessageProperties> _messageProperties = new();
    
    public MessageProperties? Get()
        => _messageProperties.Value;

    public MessageProperties InitializeIfEmpty()
    {
        if (_messageProperties.Value is not null)
        {
            return _messageProperties.Value;
        }
        
        var messageProperties = new MessageProperties(Guid.NewGuid().ToString(), 
            new Dictionary<string, object>(), string.Empty);
        _messageProperties.Value = messageProperties;
        
        return messageProperties;
    }
    
    public void Set(MessageProperties messageProperties)
        => _messageProperties.Value = messageProperties;
}

public record MessageProperties(string MessageId, IDictionary<string, object> Headers, string MessageType);