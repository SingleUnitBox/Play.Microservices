using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;
using Play.Items.Infra.Repositories.Cached.Converters;

namespace Play.Items.Infra.Repositories.Cached;

public class CachedItemRepository : IItemRepository
{
    private readonly ItemRepository _decoratedItemRepository;
    private readonly IDistributedCache _distributedCache;

    public CachedItemRepository(ItemRepository decoratedItemRepository,
        IDistributedCache distributedCache)
    {
        _decoratedItemRepository = decoratedItemRepository;
        _distributedCache = distributedCache;
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

    public async Task<Item> GetByIdAsync(AggregateRootId id)
    {
        string key = $"item-{id}";
        var cachedItem = await _distributedCache.GetStringAsync(key);
        
        Item item;
        if (string.IsNullOrEmpty(cachedItem))
        {
            item = await _decoratedItemRepository.GetByIdAsync(id);
            if (item is null)
            {
                return item;
            }

            await _distributedCache.SetStringAsync(
                key,
                JsonConvert.SerializeObject(item));
            
            return item;
        }

        // something is wrong with Item creation
        item = JsonConvert.DeserializeObject<Item>(
            cachedItem,
            new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {new ItemJsonConverter()}
            });

        return item;

        // return _memoryCache.GetOrCreateAsync(
        //     key,
        //     entry =>
        //     {
        //         entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        //
        //         return _decoratedItemRepository.GetByIdAsync(id);
        //     });
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

    public int Count()
    {
        return _decoratedItemRepository.Count();
    }
}