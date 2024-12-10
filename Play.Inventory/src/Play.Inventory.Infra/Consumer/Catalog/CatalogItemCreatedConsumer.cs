using MassTransit;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogItemCreatedConsumer : IConsumer<ItemCreated>
{
    private readonly ICatalogItemRepository _catalogItemRepository;

    public CatalogItemCreatedConsumer(ICatalogItemRepository catalogItemRepository)
    {
        _catalogItemRepository = catalogItemRepository;
    }

    public async Task Consume(ConsumeContext<ItemCreated> context)
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
            Price = context.Message.Price,
        };
        
        await _catalogItemRepository.CreateAsync(catalogItem);
    }
}