using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Policies;

public interface IWeaponPurchasePolicy
{
    bool CanCatalogItemBePurchased(CatalogItem catalogItem, InventoryItem inventoryItem);
}