using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Play.Common.Messaging.Ordering;
using Play.Inventory.Application.Events.External.Items;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Postgres.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly InventoryPostgresDbContext _dbContext;
    private readonly DbSet<CatalogItem> _catalogItems;

    public CatalogItemRepository(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
        _catalogItems = dbContext.CatalogItems;
    }
    
    public async Task CreateAsync(CatalogItem item)
    {
        await _catalogItems.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(CatalogItem item)
    {
        _catalogItems.Update(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid itemId)
    {
        var catalogItem = await _catalogItems.SingleOrDefaultAsync(i => i.Id == itemId);
        _catalogItems.Remove(catalogItem);
        await _dbContext.SaveChangesAsync();
    }

    public Task<CatalogItem> GetByIdAsync(Guid itemId)
        => _catalogItems.SingleOrDefaultAsync(i => i.Id == itemId);

    public Task<CatalogItem> GetAsync(Expression<Func<CatalogItem, bool>> predicate)
        => _catalogItems.SingleOrDefaultAsync(predicate);

    public async Task<IReadOnlyCollection<CatalogItem>> BrowseItems()
        => await _catalogItems.ToListAsync();
}