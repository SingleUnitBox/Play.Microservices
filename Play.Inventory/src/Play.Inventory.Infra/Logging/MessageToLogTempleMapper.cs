﻿using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Inventory.Application.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Application.Queries;
using Play.Items.Contracts.Events;
using Play.User.Contracts.Events;

namespace Play.Inventory.Infra.Logging;

public class MessageToLogTempleMapper : IMessageToLogTemplateMapper
{
    private readonly IReadOnlyDictionary<Type, HandlerLogTemplate> Templates = new Dictionary<Type, HandlerLogTemplate>
    {
        [typeof(ItemCreated)] = new()
        {
            Before = $"[{typeof(ItemCreated)}] " + "Starting to create catalog item '{Name}' with id '{ItemId}'.",
            After = $"[{typeof(ItemCreated)}] " + "Created catalog item '{Name}' with id '{ItemId}'.",
        },
        [typeof(ItemUpdated)] = new()
        {
            Before = $"[{typeof(ItemUpdated)}] " + "Starting to update catalog '{Name}' item with id '{ItemId}'.",
            After = $"[{typeof(ItemUpdated)}] " + "Updated catalog item '{Name}' with id '{ItemId}'.",
            OnError = new Dictionary<Type, string>()
            {
                [typeof(CatalogItemNotFoundException)] = "Catalog item '{Name}' with id '{ItemId}' was not found.",
            }
        },
        [typeof(ItemDeleted)] = new()
        {
            Before = $"[{typeof(ItemDeleted)}] " + "Starting to delete catalog item '{Name}' with id '{ItemId}'.",
            After = $"[{typeof(ItemDeleted)}] " + "Deleted catalog item '{Name}' with id '{ItemId}'.",
            OnError = new Dictionary<Type, string>()
            {
                [typeof(CatalogItemNotFoundException)] = "Catalog item '{Name}' with id '{ItemId}' was not found.",
            }
        },
        [typeof(GetCatalogItems)] = new()
        {
            Before = $"[{typeof(GetCatalogItems)}] starting query.",
            After = $"[{typeof(GetCatalogItems)}] completed query.",
        },
        [typeof(UserCreated)] = new()
        {
            Before = $"[{typeof(UserCreated)}] " + "Starting to create player '{Username}' with id '{UserId} '.",
            After = $"[{typeof(UserCreated)}] " + "Created player '{Username}' with id '{UserId}'.",
        },
        [typeof(PlayerCreated)] = new()
        {
            Before = $"[{typeof(PlayerCreated)}] " + "Starting to create money bag for player with id '{PlayerId}'.",
            After = $"[{typeof(PlayerCreated)}] " + "Created money bag for player with id '{PlayerId}'.",
        }
    };
    
    public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        => Templates.TryGetValue(message.GetType(), out var template) ? template : null;
}