using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly IMongoCollection<CatalogItem> _catalogItems;
    private readonly FilterDefinitionBuilder<CatalogItem> _filterBuilder = Builders<CatalogItem>.Filter;
    public CatalogItemRepository(IMongoDatabase database, string collectionName)
    {
        _catalogItems = database.GetCollection<CatalogItem>(collectionName);
    }
    
    public async Task CreateAsync(CatalogItem item)
    {
        await _catalogItems.InsertOneAsync(item);
    }

    public async Task UpdateAsync(CatalogItem item)
    {
        var filter = _filterBuilder.Eq(c => c.Id, item.Id);
        await _catalogItems.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(Guid itemId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, itemId);
        await _catalogItems.DeleteOneAsync(filter);
    }

    public async Task<CatalogItem> GetByIdAsync(Guid itemId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, itemId);
        return await _catalogItems.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<CatalogItem> GetAsync(Expression<Func<CatalogItem, bool>> predicate)
    {
        return await _catalogItems.Find(predicate).SingleOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<CatalogItem>> BrowseItems()
        => await _catalogItems.Find(_ => true).ToListAsync();
}