using System.Linq.Expressions;
using MongoDB.Driver;

namespace Play.User.Core.Repositories;

public class UserMongoRepository : IUserRepository
{
    private readonly IMongoCollection<Entities.User> _userCollection;
    private readonly FilterDefinitionBuilder<Entities.User> _filterBuilder = Builders<Entities.User>.Filter;

    public UserMongoRepository(IMongoDatabase mongoDatabase, string collectionName)
    {
        _userCollection = mongoDatabase.GetCollection<Entities.User>(collectionName);
    }
    
    public async Task CreateUser(Entities.User user)
    {
        await _userCollection.InsertOneAsync(user);
    }
    
    public async Task UpdateUser(Entities.User user)
    {
        var filter = _filterBuilder.Eq(u => u.Id, user.Id);
        await _userCollection.ReplaceOneAsync(filter, user);
    }

    public async Task<Entities.User> GetUserById(Guid userId)
    {
        var filter = _filterBuilder.Eq(user => user.Id, userId);
        return await _userCollection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task<Entities.User> GetUser(Expression<Func<Entities.User, bool>> filter)
    {
        return await _userCollection.Find(filter).SingleOrDefaultAsync();
    }
}