using System.Linq.Expressions;
using Play.Catalog.Service.Entities;
using Play.Common.SharedKernel.Repositories;
using Play.Common.SharedKernel.Types;

namespace Play.Catalog.Service.Repositories;

public interface IItemRepository : IMongoRepository<Item>
{
    Task CreateAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(AggregateRootId id);
    Task<Item> GetByIdAsync(AggregateRootId id);
    Task<Item> GetAsync(Expression<Func<Item, bool>> predicate);
    Task<IReadOnlyList<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate);
    Task<IReadOnlyList<Item>> GetAllAsync();
}