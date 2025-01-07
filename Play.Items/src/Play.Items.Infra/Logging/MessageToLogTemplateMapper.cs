using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Items.Application.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Queries;

namespace Play.Items.Infra.Logging;

public sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
{
    private static readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates =
        new Dictionary<Type, HandlerLogTemplate>
        {
            [typeof(CreateItem)] = new HandlerLogTemplate()
            {
                Before = "Starting to create item with id '{ItemId}'.",
                After = "Created item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemAlreadyExistException)] = "Item with id '{ItemId}' already exists."
                }
            },
            [typeof(UpdateItem)] = new HandlerLogTemplate()
            {
                Before = "Starting to update item with id '{ItemId}'.",
                After = "Updated item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            },
            [typeof(DeleteItem)] = new()
            {
                Before = "Starting to delete item with id '{ItemId}'.",
                After = "Deleted item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            },
            [typeof(DeleteItems)] = new()
            {
                Before = "Starting to delete all items.",
                After = "Deleted all items.",
            },
            [typeof(GetItem)] = new HandlerLogTemplate()
            {
                Before = "Starting to query item with id '{ItemId}'.",
                After = "Stopping to query item with id '{ItemId}'."
            },
            [typeof(GetItems)] = new HandlerLogTemplate()
            {
                Before = $"Starting query '{typeof(GetItems)}'.",
                After = $"Completed query '{typeof(GetItems)}'."
            }
        };
    
    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
    {
        var key = message.GetType();
        return Templates.TryGetValue(key, out var template) ? template : null;
    }
}