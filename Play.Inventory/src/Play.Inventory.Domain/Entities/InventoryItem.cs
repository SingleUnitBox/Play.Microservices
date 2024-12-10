using Play.Common.Abs.SharedKernel;

namespace Play.Inventory.Domain.Entities;

public class InventoryItem : AggregateRoot
{
    public Guid UserId { get; set; }
    public Guid CatalogItemId { get; set; }
    public int Quantity { get; set; }
    public DateTimeOffset AcquiredDate { get; set; }
}