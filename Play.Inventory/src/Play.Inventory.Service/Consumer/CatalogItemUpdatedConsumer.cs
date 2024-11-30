using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumer;

public class CatalogItemUpdatedConsumer : IConsumer<Contracts.CatalogItemUpdated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemUpdatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.CatalogItemUpdated> context)
    {
        var catalogItem = await _catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        if (catalogItem is null)
        {
            catalogItem = new CatalogItem
            {
                Id = context.Message.ItemId,
                Name = context.Message.Name,
                Description = context.Message.Description,
            };
            
            await _catalogItemRepository.CreateAsync(catalogItem);
        }
        else
        {
            catalogItem.Name = context.Message.Name;
            catalogItem.Description = context.Message.Description;
        
            await _catalogItemRepository.UpdateAsync(catalogItem);
        }
    }
}