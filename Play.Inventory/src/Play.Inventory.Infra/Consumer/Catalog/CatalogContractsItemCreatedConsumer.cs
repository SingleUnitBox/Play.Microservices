using MassTransit;
using Microsoft.Extensions.Logging;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts.Events;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogContractsItemCreatedConsumer(ICatalogItemRepository catalogItemRepository,
    ILogger<CatalogContractsItemCreatedConsumer> logger)
    : IConsumer<ItemCreated>
{
    public async Task Consume(ConsumeContext<ItemCreated> context)
    {
        logger.LogInformation("Consuming catalog contracts item created.");
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