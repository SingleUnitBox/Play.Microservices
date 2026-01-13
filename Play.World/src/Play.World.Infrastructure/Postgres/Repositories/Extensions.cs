using Microsoft.Extensions.DependencyInjection;
using Play.World.Domain.Repositories;

namespace Play.World.Infrastructure.Postgres.Repositories;

public static class Extensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemLocationsRepository, ItemLocationRepository>();
        services.AddScoped<IZoneRepository, ZoneRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        
        return services;
    }
}