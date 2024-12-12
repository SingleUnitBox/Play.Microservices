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
        
        var message = context.Message;
        var catalogItem = await catalogItemRepository.GetAsync(i => i.Id == context.Message.ItemId);
        if (catalogItem is not null)
        {
            return;
        }

        catalogItem = CatalogItem.Create(message.ItemId, message.Name, message.Price);
        
        await catalogItemRepository.CreateAsync(catalogItem);
    }
}