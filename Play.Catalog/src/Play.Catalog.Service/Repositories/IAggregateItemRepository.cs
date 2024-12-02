using System.Linq.Expressions;
using Play.Catalog.Service.Entities;
using Play.Common.SharedKernel.Repositories;
using Play.Common.SharedKernel.Types;

namespace Play.Catalog.Service.Repositories;

public interface IAggregateItemRepository : IMongoRepository<AggregateItem>
{
    Task<AggregateItem> GetByIdAsync(AggregateRootId aggregateRootId);
    Task<AggregateItem> GetAsync(Expression<Func<AggregateItem, bool>> predicate);
    Task CreateAsync(AggregateItem aggregateItem);
    Task UpdateAsync(AggregateItem aggregateItem);
    Task RemoveAsync(AggregateRootId aggregateRootId);
}
