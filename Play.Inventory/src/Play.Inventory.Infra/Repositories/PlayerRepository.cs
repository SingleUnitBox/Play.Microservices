using MongoDB.Driver;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly IMongoCollection<Domain.Entities.Player> _users;
    private readonly FilterDefinitionBuilder<Domain.Entities.Player> _filterBuilder = Builders<Domain.Entities.Player>.Filter;

    public PlayerRepository(IMongoDatabase database, string collectionName)
    {
        _users = database.GetCollection<Domain.Entities.Player>(collectionName);
    }
    
    public async Task CreateAsync(Domain.Entities.Player player)
    {
        await _users.InsertOneAsync(player);
    }

    public async Task UpdateAsync(Domain.Entities.Player player)
    {
        var filter = _filterBuilder.Eq(c => c.Id, player.Id);
        await _users.ReplaceOneAsync(filter, player);
    }

    public async Task DeleteAsync(Guid playerId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, playerId);
        await _users.DeleteOneAsync(filter);
    }

    public async Task<Domain.Entities.Player> GetByIdAsync(Guid playerId)
    {
        var filter = _filterBuilder.Eq(c => c.Id, playerId);
        return await _users.Find(filter).SingleOrDefaultAsync();
    }
}