using MassTransit;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogContractsItemCreatedConsumer(ICatalogItemRepository catalogItemRepository) : IConsumer<Contracts.CatalogItemCreated>
{
    public async Task Consume(ConsumeContext<Contracts.CatalogItemCreated> context)
    {
        var message = context.Message;
        var item = await catalogItemRepository.GetByIdAsync(message.ItemId);
        if (item is not null)
        {
            return;
        }
        
        item = CatalogItem.Create(message.ItemId, message.Name, message.Price);
        await catalogItemRepository.CreateAsync(item);
    }
}