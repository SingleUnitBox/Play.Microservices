using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Repositories.Cached;

public class CachedItemRepository : IItemRepository
{
    private readonly ItemRepository _decoratedItemRepository;
    private readonly IMemoryCache _memoryCache;

    public CachedItemRepository(ItemRepository decoratedItemRepository,
        IMemoryCache memoryCache)
    {
        _decoratedItemRepository = decoratedItemRepository;
        _memoryCache = memoryCache;
    }

    public async Task CreateAsync(Item item)
    {
        await _decoratedItemRepository.CreateAsync(item);
    }

    public async Task UpdateAsync(Item item)
    {
        await _decoratedItemRepository.UpdateAsync(item);
    }

    public async Task DeleteAsync(AggregateRootId id)
    {
        await _decoratedItemRepository.DeleteAsync(id);
    }

    public Task<Item> GetByIdAsync(AggregateRootId id)
    {
        string key = $"item-{id}";
        return _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                return _decoratedItemRepository.GetByIdAsync(id);
            });
    }

    public Task<Item> GetAsync(Expression<Func<Item, bool>> predicate)
    {
        return _decoratedItemRepository.GetAsync(predicate);
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate)
    {
        return await _decoratedItemRepository.GetAllAsync(predicate);
    }

    public async Task<IReadOnlyList<Item>> GetAllAsync()
    {
        return await _decoratedItemRepository.GetAllAsync();
    }
}