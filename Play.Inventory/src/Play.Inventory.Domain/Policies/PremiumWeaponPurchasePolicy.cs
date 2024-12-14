using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Policies;

public class PremiumWeaponPurchasePolicy : IWeaponPurchasePolicy
{
    public bool CanCatalogItemBePurchased(CatalogItem catalogItem, InventoryItem inventoryItem)
    {
        if (catalogItem.Id == inventoryItem.CatalogItemId
            && inventoryItem.Quantity >= 2)
        {
            return false;
        }
        
        return true;
    }
}