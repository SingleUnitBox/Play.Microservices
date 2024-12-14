using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Policies.Factories;

public class WeaponPurchasePolicyFactory : IWeaponPurchasePolicyFactory
{
    public IWeaponPurchasePolicy Create(MoneyBag moneyBag)
    {
        return moneyBag.Gold switch
        {
            >= 500 => new PremiumWeaponPurchasePolicy(),
            _ => new BasicWeaponPurchasePolicy()
        };
    }
}