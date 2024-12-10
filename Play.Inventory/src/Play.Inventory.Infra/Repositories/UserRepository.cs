using MongoDB.Driver;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<Domain.Entities.User> _users;
    private readonly FilterDefinitionBuilder<Domain.Entities.User> _filterBuilder = Builders<Domain.Entities.User>.Filter;

    public UserRepository(IMongoDatabase database, string collectionName)
    {
        _users = database.GetCollection<Domain.Entities.User>(collectionName);
    }
    
    public async Task CreateAsync(Domain.Entities.User user)
    {
        await _users.InsertOneAsync(user);
    }

    public async Task UpdateAsync(Domain.Entities.User user)
    {
        var filter = _filterBuilder.Eq(c => c.Id, user.Id);
        await _users.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteAsync(Guid userId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, userId);
        await _users.DeleteOneAsync(filter);
    }

    public async Task<Domain.Entities.User> GetByIdAsync(Guid userId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, userId);
        return await _users.Find(filter).SingleOrDefaultAsync();
    }
}