using System.Linq.Expressions;
using Play.Common.Abs.SharedKernel.Repositories;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.Entities;

namespace Play.Items.Domain.Repositories;

public interface IItemRepository : IMongoRepository<Item>
{
    Task CreateAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(AggregateRootId id);
    Task<Item> GetByIdAsync(AggregateRootId id);
    Task<Item> GetAsync(Expression<Func<Item, bool>> predicate);
    Task<IReadOnlyList<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate);
    Task<IReadOnlyList<Item>> GetAllAsync();
    int Count();
}