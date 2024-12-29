using MassTransit;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogItemUpdatedConsumer : IConsumer<ItemUpdated>
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemUpdatedConsumer(ICatalogItemRepository catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }
    
    public async Task Consume(ConsumeContext<ItemUpdated> context)
    {
        var catalogItem = await _catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        // if (catalogItem is null)
        // {
        //     catalogItem = new CatalogItem
        //     {
        //         Id = context.Message.ItemId,
        //         Name = context.Message.Name,
        //         Price = context.Message.Description,
        //     };
        //     
        //     await _catalogItemRepository.CreateAsync(catalogItem);
        // }
        // else
        // {
        //     catalogItem.Name = context.Message.Name;
        //     catalogItem.Price = context.Message.Description;
        //
        //     await _catalogItemRepository.UpdateAsync(catalogItem);
        // }
    }
}