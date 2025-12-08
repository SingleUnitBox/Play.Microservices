using Play.Common.Abs.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.External.Items.Handlers;

public class ItemCreatedHandler : IEventHandler<ItemCreated>
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    
    public ItemCreatedHandler(ICatalogItemRepository catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    
    public async Task HandleAsync(ItemCreated @event)
    {
        var item = await _catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (item is not null)
        {
            return;
        }
        
        item = CatalogItem.Create(@event.ItemId, @event.Name, @event.Price, @event.Version);
        await _catalogItemRepository.CreateAsync(item);
    }
}