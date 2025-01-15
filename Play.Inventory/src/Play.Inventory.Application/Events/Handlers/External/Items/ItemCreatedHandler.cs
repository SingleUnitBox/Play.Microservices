using Play.Common.Abs.Events;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Application.Events.Handlers.External.Items;

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
        
        item = CatalogItem.Create(@event.ItemId, @event.Name, @event.Price);
        await _catalogItemRepository.CreateAsync(item);
    }
}