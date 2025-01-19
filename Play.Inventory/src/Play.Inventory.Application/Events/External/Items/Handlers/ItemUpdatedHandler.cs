using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.External.Items.Handlers;

public class ItemUpdatedHandler(ICatalogItemRepository catalogItemRepository) : IEventHandler<ItemUpdated>
{
    public async Task HandleAsync(ItemUpdated @event)
    {
        var item = await catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(@event.ItemId);
        }
        
        item.Name = @event.Name;
        item.Price = @event.Price;
        
        await catalogItemRepository.UpdateAsync(item);
    }
}
