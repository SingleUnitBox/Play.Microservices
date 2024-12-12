using MassTransit;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;
using Play.Items.Contracts;

namespace Play.Inventory.Infra.Consumer.Catalog;

public class CatalogContractsItemDeletedConsumer(ICatalogItemRepository catalogItemRepository) : IConsumer<Contracts.CatalogItemDeleted>
{
    public async Task Consume(ConsumeContext<Contracts.CatalogItemDeleted> context)
    {
        var message = context.Message;
        var item = await catalogItemRepository.GetByIdAsync(message.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(message.ItemId);
        }
        
        await catalogItemRepository.DeleteAsync(item.Id);
    }
}