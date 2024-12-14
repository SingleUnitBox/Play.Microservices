using Microsoft.Extensions.DependencyInjection;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Postgres.Repositories;

public static class Extensions
{
    public static IServiceCollection AddPostgresRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
        services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IMoneyBagRepository, MoneyBagRepository>();
        
        return services;
    }
}