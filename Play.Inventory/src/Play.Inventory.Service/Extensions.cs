using Play.Inventory.Application;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Service;

public static class Extensions
{
    public static InventoryItemDto AsDto(this InventoryItem inventoryItem, string name, string description)
        => new InventoryItemDto(inventoryItem.CatalogItemId,
            name, description, inventoryItem.Quantity, inventoryItem.AcquiredDate);
}