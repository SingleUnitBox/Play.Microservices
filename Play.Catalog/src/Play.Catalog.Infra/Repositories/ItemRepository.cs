using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Catalog.Domain.Entities;
using Play.Catalog.Domain.Repositories;
using Play.Common

namespace Play.Catalog.Infra.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IMongoCollection<Item> _itemsCollection;
    private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

    public ItemRepository(IMongoDatabase database, string collectionName)
    {
        _itemsCollection = database.GetCollection<Item>(collectionName);
    }
    
    public async Task CreateAsync(Item item)
        => await _itemsCollection.InsertOneAsync(item);

    public async Task UpdateAsync(Item item)
    {
        var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
        await _itemsCollection.ReplaceOneAsync(filter, item);
    }
    
    public async Task DeleteAsync(AggregateRootId id)
    {
        var filter = _filterBuilder.Eq(item => item.Id, id);
        await _itemsCollection.DeleteOneAsync(filter);
    }

    public async Task<Item> GetByIdAsync(AggregateRootId id)
    {
        var filter = _filterBuilder.Eq(i => i.Id, id);
        return await _itemsCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<Item> GetAsync(Expression<Func<Item, bool>> predicate)
        => await _itemsCollection.Find(predicate).SingleOrDefaultAsync();
    

    public async Task<IReadOnlyList<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate)
        => await _itemsCollection.Find(predicate).ToListAsync();

    public async Task<IReadOnlyList<Item>> GetAllAsync()
    {
        var items = await _itemsCollection.Find(_filterBuilder.Empty).ToListAsync();
        return items;
    }
    
}