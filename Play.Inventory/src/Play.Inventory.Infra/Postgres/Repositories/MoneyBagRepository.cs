using Microsoft.EntityFrameworkCore;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Postgres.Repositories;

public class MoneyBagRepository : IMoneyBagRepository
{
    private readonly InventoryPostgresDbContext _dbContext;
    private readonly DbSet<MoneyBag> _moneyBags;

    public MoneyBagRepository(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
        _moneyBags = _dbContext.MoneyBags;
    }
    public async Task CreateMoneyBag(MoneyBag moneyBag)
    {
        await _moneyBags.AddAsync(moneyBag);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateMoneyBag(MoneyBag moneyBag)
    {
        _moneyBags.Update(moneyBag);
        await _dbContext.SaveChangesAsync();
    }

    public Task<MoneyBag> GetMoneyBagByUserId(Guid playerId)
        => _moneyBags.SingleOrDefaultAsync(x => x.PlayerId == playerId);
}