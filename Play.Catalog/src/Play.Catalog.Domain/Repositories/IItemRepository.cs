using System.Linq.Expressions;
using Play.Catalog.Domain.Entities;
using Play.Common.Abstractions.SharedKernel.Repositories;
using Play.Common.Abstractions.SharedKernel.Types;

namespace Play.Catalog.Domain.Repositories;

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