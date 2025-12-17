using Play.Common.Abs.Events;
using Play.Inventory.Application.Exceptions;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Events.External.Items.Handlers;

public class ArtifactAddedHandler(ICatalogItemRepository catalogItemRepository) : IEventHandler<ArtifactAdded>
{
    public async Task HandleAsync(ArtifactAdded @event)
    {
        var item = await catalogItemRepository.GetByIdAsync(@event.ItemId);
        if (item is null)
        {
            throw new CatalogItemNotFoundException(@event.ItemId);
        }

        if (!string.IsNullOrWhiteSpace(@event.Artifact))
        {
            item.HasArtifact = true;
            item.LastKnownVersion = @event.Version;
            await catalogItemRepository.UpdateAsync(item);
        }
    }
}