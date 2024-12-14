using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.SharedKernel.Types;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Postgres.Repositories;

public class InventoryItemRepository : IInventoryItemRepository
{
    private readonly InventoryPostgresDbContext _dbContext;
    private readonly DbSet<InventoryItem> _items;

    public InventoryItemRepository(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
        _items = _dbContext.InventoryItems;
    }
    public async Task CreateAsync(InventoryItem item)
    {
        await _items.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(InventoryItem item)
    {
        _items.Update(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(AggregateRootId itemId)
    {
        var item = await _items.SingleOrDefaultAsync(i => i.Id == itemId);
        _items.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public Task<InventoryItem> GetByIdAsync(AggregateRootId itemId)
        => _items.SingleOrDefaultAsync(i => i.Id == itemId);

    public Task<InventoryItem> GetInventory(Expression<Func<InventoryItem, bool>> predicate)
        => _items.SingleOrDefaultAsync(predicate);
}