using MassTransit;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogItemDeletedConsumer : IConsumer<Contracts.CatalogItemDeleted>
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemDeletedConsumer(ICatalogItemRepository catalogItemRepository)
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

        await _catalogItemRepository.DeleteAsync(catalogItem.Id);
    }
}