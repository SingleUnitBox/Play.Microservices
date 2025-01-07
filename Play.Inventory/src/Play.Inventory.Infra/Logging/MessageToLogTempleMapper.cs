using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Application.Queries;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Logging;

public class MessageToLogTempleMapper : IMessageToLogTemplateMapper
{
    private readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates = new Dictionary<Type, HandlerLogTemplate>
    {
        [typeof(ItemCreated)] = new()
        {
            Before = "Starting to create catalog item with id '{ItemId}'.",
            After = "Created catalog item with id '{ItemId}'.",
        },
        [typeof(ItemUpdated)] = new()
        {
            Before = "Starting to update catalog item with id '{ItemId}'.",
            After = "Updated catalog item with id '{ItemId}'.",
            OnError = new Dictionary<Type, string>()
            {
                [typeof(CatalogItemNotFoundException)] = "Catalog item with id '{ItemId}' was not found.",
            }
        },
        [typeof(ItemDeleted)] = new()
        {
            Before = "Starting to delete catalog item with id '{ItemId}'.",
            After = "Deleted catalog item with id '{ItemId}'.",
            OnError = new Dictionary<Type, string>()
            {
                [typeof(CatalogItemNotFoundException)] = "Catalog item with id '{ItemId}' was not found.",
            }
        },
        [typeof(GetCatalogItems)] = new()
        {
            Before = $"Starting query '{typeof(GetCatalogItems)}'.",
            After = $"Completed query '{typeof(GetCatalogItems)}'.",
        }
    };
    
    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        => Templates.TryGetValue(message.GetType(), out var template) ? template : null;
}