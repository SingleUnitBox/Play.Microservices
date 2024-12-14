using Microsoft.Extensions.DependencyInjection;
using Play.Inventory.Domain.Policies.Factories;

namespace Play.Inventory.Domain.Policies;

public static class Extensions
{
    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddSingleton<IWeaponPurchasePolicyFactory, WeaponPurchasePolicyFactory>();
        
        return services;
    }
}