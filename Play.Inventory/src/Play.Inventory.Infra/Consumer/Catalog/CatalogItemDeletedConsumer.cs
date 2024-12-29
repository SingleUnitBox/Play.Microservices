using MassTransit;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogItemDeletedConsumer : IConsumer<ItemDeleted>
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemDeletedConsumer(ICatalogItemRepository catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<ItemDeleted> context)
    {
        var catalogItem = await _catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        if (catalogItem == null)
        {
            return;
        }

        await _catalogItemRepository.DeleteAsync(catalogItem.Id);
    }
}