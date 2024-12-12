using MassTransit;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogContractsItemUpdatedConsumer(ICatalogItemRepository catalogItemRepository) : IConsumer<Contracts.CatalogItemUpdated>
{
    public async Task Consume(ConsumeContext<Contracts.CatalogItemUpdated> context)
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