using MongoDB.Driver;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Infra.Queries;
using Play.Inventory.Infra.Queries.Handlers;

namespace Play.Inventory.Infra.Mongo.Queries;

public class MongoHandlerDataAccessLayer : IDataAccessLayer
{
    private readonly IMongoDatabase _dbContext;

    public MongoHandlerDataAccessLayer(IMongoDatabase dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<CatalogItem>> BrowseItems()
        => await _dbContext
            .GetCollection<CatalogItem>("catalogItems")
            .Find(i => true)
            .ToListAsync();
}