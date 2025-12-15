using Microsoft.Extensions.DependencyInjection;
using Play.Items.Application.Factories;
using Play.Items.Domain.Repositories;
using Play.Items.Infra.Postgres.Factories;

namespace Play.Items.Infra.Postgres.Repositories;

public static class Extensions
{
    public static IServiceCollection AddPostgresRepositories(this IServiceCollection services)
    {
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<ICrafterRepository, CrafterRepository>();
        services.AddScoped<IElementRepository, ElementRepository>();
        
        services.AddScoped<IArtifactDefinitionRepository, ArtifactDefinitionRepository>();
        services.AddScoped<IArtifactFactory, ArtifactFactory>();
        
        return services;
    }
}