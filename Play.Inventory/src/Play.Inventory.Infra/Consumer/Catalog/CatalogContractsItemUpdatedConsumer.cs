using MassTransit;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogContractsItemUpdatedConsumer(ICatalogItemRepository catalogItemRepository)
    : IConsumer<ItemUpdated>
{
    public async Task Consume(ConsumeContext<ItemUpdated> context)
    {
        var message = context.Message;
        var item = await catalogItemRepository.GetByIdAsync(message.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(message.ItemId);
        }
        
        item.Name = message.Name;
        item.Price = message.Price;
        
        await catalogItemRepository.UpdateAsync(item);
    }
}