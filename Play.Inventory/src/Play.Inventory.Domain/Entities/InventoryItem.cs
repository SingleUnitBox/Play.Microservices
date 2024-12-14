using Play.Common.Abs.SharedKernel;
using Play.Common.Abs.SharedKernel.Types;

namespace Play.Inventory.Domain.Entities;

public class InventoryItem : AggregateRoot
{
    public Guid PlayerId { get; private set; }
    public Guid CatalogItemId { get; private set; }
    public int Quantity { get; private set; }
    public DateTimeOffset AcquiredDate { get; private set; }

    private InventoryItem(Guid catalogItemId, Guid playerId, int quantity, DateTimeOffset acquiredDate)
    {
        Id = new AggregateRootId();
        PlayerId = playerId;
        CatalogItemId = catalogItemId;
        Quantity = quantity;
        AcquiredDate = acquiredDate;
    }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public static InventoryItem Create(Guid catalogItemId, Guid playerId, int quantity, DateTimeOffset acquiredDate)
        => new InventoryItem(catalogItemId, playerId, quantity, acquiredDate);
}