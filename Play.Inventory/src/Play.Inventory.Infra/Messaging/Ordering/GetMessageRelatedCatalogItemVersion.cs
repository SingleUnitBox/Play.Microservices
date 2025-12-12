using Microsoft.EntityFrameworkCore;
using Play.Common.Messaging.Ordering;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Infra.Postgres;

namespace Play.Inventory.Infra.Messaging.Ordering;

public class GetMessageRelatedCatalogItemVersion(InventoryPostgresDbContext dbContext)
    : IGetMessageRelatedEntityVersion<ItemCreated>
{
    public async Task<int?> GetEntityVersionAsync(ItemCreated message, CancellationToken cancellationToken = default)
    {
        var item = await dbContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == message.ItemId);
        if (item is null)
        {
            return null;
        }
        
        return item.LastKnownVersion;
    }
}