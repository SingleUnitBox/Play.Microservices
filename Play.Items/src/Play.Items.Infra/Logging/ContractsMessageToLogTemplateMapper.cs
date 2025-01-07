using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Commands;
using DeleteItems = Play.Items.Application.Commands.DeleteItems;

namespace Play.Items.Infra.Logging;

public sealed class ContractsMessageToLogTemplateMapper : IMessageToLogTemplateMapper
{
    private static IReadOnlyDictionary<Type, HandlerLogTemplate> Templates = new Dictionary<Type, HandlerLogTemplate>()
    {
        [typeof(CreateItem)] = new()
        {
            Before = "Starting to create item with id '{ItemId}'.",
            After = "Created item with id '{ItemId}'.",
            OnError = new Dictionary<Type, string>()
            {
                [typeof(ItemAlreadyExistException)] = "Item with id '{ItemId}' already exists."
            }
        },
        [typeof(UpdateItem)] = new()
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
        }
    };

    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        => Templates.TryGetValue(message.GetType(), out var template) ? template : null;
}