using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.External.Items.Handlers;

public class ItemDeletedHandler(ICatalogItemRepository catalogItemRepository) : IEventHandler<ItemDeleted>
{
    public async Task HandleAsync(ItemDeleted @event)
    {
        var item = await catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(@event.ItemId);
        }
        
        item.Delete();
        item.LastKnownVersion = @event.Version;
        
        await catalogItemRepository.UpdateAsync(item);
    }
}