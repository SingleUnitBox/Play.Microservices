using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumer.Catalog;

public class CatalogItemCreatedConsumer : IConsumer<Contracts.CatalogItemCreated>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemCreatedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.CatalogItemCreated> context)
    {
        var catalogItem = await _catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        if (catalogItem is not null)
        {
            return;
        }

        catalogItem = new CatalogItem
        {
            Id = context.Message.ItemId,
            Name = context.Message.Name,
            Description = context.Message.Description,
        };
        
        await _catalogItemRepository.CreateAsync(catalogItem);
    }
}