﻿using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Application.Events.Handlers.External.Items;

public class ItemDeletedHandler(ICatalogItemRepository catalogItemRepository) : IEventHandler<ItemDeleted>
{
    public async Task HandleAsync(ItemDeleted @event)
    {
        var item = await catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(@event.ItemId);
        }

        await catalogItemRepository.DeleteAsync(item.Id);
    }
}