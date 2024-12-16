using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Common.Abs.Queries;
using Play.Common.Queries;
using Play.Common.Settings;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Infra.Mongo.Queries;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Queries;

namespace Play.Inventory.Infra.Queries.Handlers;

public static class Extensions
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        //services.AddScoped<IQueryHandler<GetCatalogItems, IReadOnlyCollection<ItemDto>>, GetCatalogItemsHandler>();
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IDataAccessLayerResolver, DataAccessLayerResolver>();
        services.AddScoped<IDataAccessLayer>(sp =>
        {
            var scope = sp.CreateScope();
            var mongoDbSettings = scope.ServiceProvider.GetRequiredService<IConfiguration>()
             .GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            if (mongoDbSettings.Enabled)
            {
                return new MongoHandlerDataAccessLayer(scope.ServiceProvider.GetRequiredService<IMongoDatabase>());
            }

            return new PostgresHandlerDataAccessLayer(scope.ServiceProvider.GetRequiredService<InventoryPostgresDbContext>());
        });
        
        return services;
    }
}