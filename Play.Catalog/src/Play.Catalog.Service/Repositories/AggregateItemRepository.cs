using System.Linq.Expressions;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Common.SharedKernel.Types;

namespace Play.Catalog.Service.Repositories;

public class AggregateItemRepository : IAggregateItemRepository
{
    private readonly IMongoCollection<AggregateItem> _aggregateItemsCollection;
    
    public AggregateItemRepository(IMongoDatabase database, string collectionName)
    {
        _aggregateItemsCollection = database.GetCollection<AggregateItem>(collectionName);
    }
    
    public Task<AggregateItem> GetByIdAsync(AggregateRootId aggregateRootId)
    {
        throw new NotImplementedException();
    }

    public Task<AggregateItem> GetAsync(Expression<Func<AggregateItem, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(AggregateItem aggregateItem)
    {
        await _aggregateItemsCollection.InsertOneAsync(aggregateItem);
    }

    public Task UpdateAsync(AggregateItem aggregateItem)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(AggregateRootId aggregateRootId)
    {
        throw new NotImplementedException();
    }
}
