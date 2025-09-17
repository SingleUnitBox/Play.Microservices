using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Postgres.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly DbSet<Item> _items;
    private readonly ItemsPostgresDbContext _dbContext;

    public ItemRepository(ItemsPostgresDbContext dbContext)
    {
        _items = dbContext.Items;
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(Item item)
    {
        await _items.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        _items.Update(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(AggregateRootId id)
    {
        var item = await _items.SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            throw new ItemNotFoundException(id);
        }

        _items.Remove(item);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Item> GetByIdAsync(AggregateRootId id)
    {
        var item = await _items
            //.Include(i => i.Crafter)
            .SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return null;
        }
        
        return item;
    }

    public async Task<Item> GetAsync(Expression<Func<Item, bool>> predicate)
    {
        var item = await _items
            .Include(i => i.Crafter)
            .SingleOrDefaultAsync(predicate);
        if (item is null)
        {
            throw new ItemNotFoundException();
        }
        
        return item;
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate)
    {
        var items = await _items.Where(predicate).ToListAsync();
        return items;
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync()
        => await _items.ToListAsync();

    public int Count()
        =>  _items.Count();
}