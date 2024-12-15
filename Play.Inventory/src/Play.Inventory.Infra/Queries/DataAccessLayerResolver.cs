using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Common.Settings;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Infra.Mongo.Queries;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Queries;
using Play.Inventory.Infra.Queries.Handlers;
using SharpCompress.Crypto;

namespace Play.Inventory.Infra.Queries;

public class DataAccessLayerResolver : IDataAccessLayerResolver
{
    private readonly IDataAccessLayer _dataAccessLayer;

    public DataAccessLayerResolver(IDataAccessLayer dataAccessLayer)
    {
        _dataAccessLayer = dataAccessLayer;
    }
    
    public async Task<IReadOnlyCollection<CatalogItem>> Resolve()
        => await _dataAccessLayer.BrowseItems();
}

// public class DataAccessLayerResolver : IDataAccessLayerResolver
// {
//     private readonly IServiceProvider _serviceProvider;
//
//     public DataAccessLayerResolver(IServiceProvider serviceProvider)
//     {
//         _serviceProvider = serviceProvider;
//     }
//     
//     public async Task<IReadOnlyCollection<CatalogItem>> Resolve()
//     {
//         using var scope = _serviceProvider.CreateScope();
//         
//         var mongoDbSettings = scope.ServiceProvider.GetRequiredService<IConfiguration>()
//             .GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
//
//         IDataAccessLayer dal;
//         if (mongoDbSettings.Enabled)
//         {
//             dal = new MongoHandlerDataAccessLayer(scope.ServiceProvider.GetRequiredService<IMongoDatabase>());
//         }
//         else
//         {
//             dal = new PostgresHandlerDataAccessLayer(scope.ServiceProvider
//                      .GetRequiredService<InventoryPostgresDbContext>());
//         }
//         
//         return await dal.BrowseItems();
//     }
// }