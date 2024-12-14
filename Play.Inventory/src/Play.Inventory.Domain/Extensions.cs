using Microsoft.Extensions.DependencyInjection;
using Play.Inventory.Domain.Policies;

namespace Play.Inventory.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddPolicies();
        
        return services;
    }
}