using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumer.Catalog;

public class CatalogItemDeletedConsumer : IConsumer<Contracts.CatalogItemDeleted>
{
    private readonly IRepository<CatalogItem> _catalogItemRepository;

    public CatalogItemDeletedConsumer(IRepository<CatalogItem> catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<Contracts.CatalogItemDeleted> context)
    {
        var catalogItem = await _catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        if (catalogItem == null)
        {
            return;
        }

        await _catalogItemRepository.RemoveAsync(catalogItem.Id);
    }
}