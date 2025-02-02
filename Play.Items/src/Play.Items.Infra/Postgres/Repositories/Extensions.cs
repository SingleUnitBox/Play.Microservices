using Microsoft.Extensions.DependencyInjection;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Postgres.Repositories;

public static class Extensions
{
    public static IServiceCollection AddPostgresRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICrafterRepository, CrafterRepository>();
        
        return services;
    }
}