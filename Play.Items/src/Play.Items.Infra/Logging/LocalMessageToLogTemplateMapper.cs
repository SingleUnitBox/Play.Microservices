using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Items.Application.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Queries;

namespace Play.Items.Infra.Logging;

public sealed class LocalMessageToLogTemplateMapper : IMessageToLogTemplateMapper
{
    private static readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates =
        new Dictionary<Type, HandlerLogTemplate>
        {
            [typeof(Contracts.Commands.CreateItem)] = new HandlerLogTemplate()
            {
                Before = $"[{typeof(Contracts.Commands.CreateItem)}] " + "Starting to create item with id '{ItemId}'." ,
                After = $"[{typeof(Contracts.Commands.CreateItem)}] " +"Created item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemAlreadyExistException)] = "Item with id '{ItemId}' already exists."
                }
            },
            [typeof(Contracts.Commands.UpdateItem)] = new HandlerLogTemplate()
            {
                Before = $"[{typeof(Contracts.Commands.UpdateItem)}] " + "Starting to update item with id '{ItemId}'.",
                After = $"[{typeof(Contracts.Commands.UpdateItem)}] " + "Updated item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            },
            [typeof(DeleteItem)] = new()
            {
                Before = $"[{typeof(Contracts.Commands.DeleteItem)}] " + "Starting to delete item with id '{ItemId}'.",
                After = $"[{typeof(Contracts.Commands.DeleteItem)}] " + "Deleted item with id '{ItemId}'.",
                OnError = new Dictionary<Type, string>()
                {
                    [typeof(ItemNotFoundException)] = "Item with id '{ItemId}' was not found.",
                }
            },
            [typeof(Contracts.Commands.DeleteItems)] = new()
            {
                Before = $"[{typeof(Contracts.Commands.DeleteItems)}] " + "Starting to delete all items.",
                After = $"[{typeof(Contracts.Commands.DeleteItems)}] " + "Deleted all items.",
            },
            [typeof(GetItem)] = new HandlerLogTemplate()
            {
                Before = $"[{typeof(GetItem)}] " + "Starting to query item with id '{ItemId}'.",
                After = $"[{typeof(GetItem)}] " + "Stopping to query item with id '{ItemId}'."
            },
            [typeof(GetItems)] = new HandlerLogTemplate()
            {
                Before = $"[{typeof(GetItem)}] starting query.",
                After = $"[{typeof(GetItem)}] completed query.",
            }
        };

    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
    {
        var key = message.GetType();
        return Templates.TryGetValue(key, out var template) ? template : null;
    }
}