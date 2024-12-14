using MongoDB.Driver;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Repositories;

public class MoneyBagRepository : IMoneyBagRepository
{
    private readonly IMongoCollection<MoneyBag> _moneyBagCollection;
    private readonly FilterDefinitionBuilder<MoneyBag> _filterDefinitionBuilder = Builders<MoneyBag>.Filter;

    public MoneyBagRepository(IMongoDatabase database, string collectionName)
    {
        _moneyBagCollection = database.GetCollection<MoneyBag>(collectionName);
    }
    
    public async Task CreateMoneyBag(MoneyBag moneyBag)
    {
        await _moneyBagCollection.InsertOneAsync(moneyBag);
    }

    public async Task UpdateMoneyBag(MoneyBag moneyBag)
    {
        var filter = _filterDefinitionBuilder.Eq(m => m.PlayerId, moneyBag.PlayerId);
        await _moneyBagCollection.ReplaceOneAsync(filter, moneyBag);
    }

    public async Task<MoneyBag> GetMoneyBagByUserId(Guid playerId)
    {
        var filter = _filterDefinitionBuilder.Eq(m => m.PlayerId, playerId);
        return await _moneyBagCollection.Find(filter).SingleOrDefaultAsync();
    }
}