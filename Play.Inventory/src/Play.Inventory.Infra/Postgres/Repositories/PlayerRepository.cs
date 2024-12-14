using Microsoft.EntityFrameworkCore;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Infra.Postgres.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly InventoryPostgresDbContext _dbContext;
    private readonly DbSet<Player> _players;

    public PlayerRepository(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
        _players = _dbContext.Players;
    }
    
    public async Task CreateAsync(Player player)
    {
        await _players.AddAsync(player);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Player player)
    {
        _players.Update(player);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid playerId)
    {
        var player = await _players.SingleOrDefaultAsync(p => p.Id == playerId);
        _players.Remove(player);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Player> GetByIdAsync(Guid playerId)
        => _players.SingleOrDefaultAsync(p => p.Id == playerId);
}