using Microsoft.EntityFrameworkCore;
using Play.Common.Messaging.Ordering;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Infra.Postgres;

namespace Play.Inventory.Infra.Messaging.Ordering;

public class GetMessageRelatedCatalogItemVersion(InventoryPostgresDbContext dbContext) : IGetMessageRelatedEntityVersion<ItemCreated>,
        IGetMessageRelatedEntityVersion<ItemUpdated>,
        IGetMessageRelatedEntityVersion<ArtifactAdded>

{
    public Task<int?> GetEntityVersionAsync(ItemCreated message, CancellationToken cancellationToken = default)
    {
        return GetEntityVersionAsync(message.ItemId, cancellationToken);
    }

    public Task<int?> GetEntityVersionAsync(ArtifactAdded message, CancellationToken cancellationToken = default)
    {
        return GetEntityVersionAsync(message.ItemId, cancellationToken);
    }

    public Task<int?> GetEntityVersionAsync(ItemUpdated message, CancellationToken cancellationToken = default)
    {
        return GetEntityVersionAsync(message.ItemId, cancellationToken);
    }

    private async Task<int?> GetEntityVersionAsync(Guid itemId, CancellationToken cancellationToken)
    {
        var item = await dbContext.CatalogItems.SingleOrDefaultAsync(i => i.Id == itemId, cancellationToken);

        return item?.LastKnownVersion;
    }
}