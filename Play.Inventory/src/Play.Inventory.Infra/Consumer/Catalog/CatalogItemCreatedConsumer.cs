using MassTransit;
using Microsoft.Extensions.Logging;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogItemCreatedConsumer(
    ICatalogItemRepository catalogItemRepository,
    ILogger<CatalogItemCreatedConsumer> logger) : IConsumer<ItemCreated>
{
    public async Task Consume(ConsumeContext<ItemCreated> context)
    {
        logger.LogInformation($"Consuming message - {context.Message.GetType().Name}");
        
        var catalogItem = await catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
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
        
        await catalogItemRepository.CreateAsync(catalogItem);
    }
}