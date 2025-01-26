using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Operation.Api.Events;
using Play.Operation.Api.Events.RejectedEvents;

namespace Play.Operation.Api.Logging;

public class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
{
    private readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates
        = new Dictionary<Type, HandlerLogTemplate>
        {
            [typeof(ItemCreated)] = new HandlerLogTemplate
            {
                Before = $"[{typeof(ItemCreated)}] " + "Starting to create catalog item '{Name}' with id '{ItemId}'.",
                After = $"[{typeof(ItemCreated)}] " + "Created catalog item '{Name}' with id '{ItemId}'.",
            },
            [typeof(CreateItemRejected)] = new()
            {
                
            }
        };
    
    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
    {
        var key = message.GetType();
        return Templates.TryGetValue(key, out var template) ? template : null;
    }
}