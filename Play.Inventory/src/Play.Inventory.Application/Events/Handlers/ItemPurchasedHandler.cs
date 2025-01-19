using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.Handlers;

public class ItemPurchasedHandler(IMoneyBagRepository moneyBagRepository,
    ICatalogItemRepository catalogItemRepository) : IEventHandler<ItemPurchased>
{
    public async Task HandleAsync(ItemPurchased @event)
    {
        var moneyBag = await moneyBagRepository.GetMoneyBagByPlayerId(@event.PlayerId);
        if (moneyBag is null)
        {
            throw new MoneyBagNotFoundException(@event.PlayerId);
        }
        
        var catalogItem = await catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (catalogItem is null)
        {
            throw new CatalogItemNotFoundException(@event.ItemId);
        }
        
        moneyBag.SubtractGold(catalogItem.Price);
        await moneyBagRepository.UpdateMoneyBag(moneyBag);
    }
}