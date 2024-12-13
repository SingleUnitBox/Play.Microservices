using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Common.Abs.SharedKernel;
using Play.Common.Abs.SharedKernel.Types;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    private readonly IMongoCollection<InventoryItem> _inventoryItems;
    private readonly FilterDefinitionBuilder<InventoryItem> _filterBuilder = Builders<InventoryItem>.Filter;
    
    public InventoryItemRepository(IMongoDatabase database, string collectionName)
    {
        _inventoryItems = database.GetCollection<InventoryItem>(collectionName);
    }

    public async Task CreateAsync(InventoryItem item)
    {
        await _inventoryItems.InsertOneAsync(item);
    }

    public async Task UpdateAsync(InventoryItem item)
    {
        var filter = _filterBuilder.Eq(i => i.Id, item.Id);
        await _inventoryItems.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteAsync(AggregateRootId itemId)
    {
        var filter = _filterBuilder.Eq(i => i.Id, itemId);
        await _inventoryItems.DeleteOneAsync(filter);
    }

    public async Task<InventoryItem> GetByIdAsync(AggregateRootId itemId)
    {
        var filter = _filterBuilder.Eq(i => i.Id, itemId);
        return await _inventoryItems.Find(filter).SingleOrDefaultAsync();
    }

    public Task<InventoryItem> GetInventory(Expression<Func<InventoryItem, bool>> predicate)
        => _inventoryItems.Find(predicate).SingleOrDefaultAsync();
}