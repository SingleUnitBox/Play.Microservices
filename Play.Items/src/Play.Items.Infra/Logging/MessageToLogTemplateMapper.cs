using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Items.Application.Commands;
using Play.Items.Application.Exceptions;

namespace Play.Items.Infra.Logging;

public sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
{
    private static readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates =
        new Dictionary<Type, HandlerLogTemplate>
        {
            [typeof(CreateItem)] = new HandlerLogTemplate()
            {
                Before = "Starting to create item with id '{ItemId}'.",
                After = "Stopping to create item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemAlreadyExistException)] = "Item with id '{ItemId}' already exists."
                }
            },
            [typeof(UpdateItem)] = new HandlerLogTemplate()
            {
                Before = "Starting to update item with id '{ItemId}'.",
                After = "Stopping to update item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            },
            [typeof(DeleteItem)] = new()
            {
                Before = "Starting to delete item with id '{ItemId}'.",
                After = "Stopping to delete item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            }
        };
    
    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
    {
        var key = message.GetType();
        return Templates.TryGetValue(key, out var template) ? template : null;
    }
}