using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Policies.Factories;

public interface IWeaponPurchasePolicyFactory
{
    IWeaponPurchasePolicy Create(MoneyBag moneyBag);
}