using Microsoft.EntityFrameworkCore;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;

namespace Play.World.Infrastructure.Postgres.Repositories;

internal sealed class PlayerRepository : IPlayerRepository
{
    private readonly DbSet<Player> _players;
    private readonly WorldPostgresDbContext _dbContext;

    public PlayerRepository(WorldPostgresDbContext dbContext)
    {
        _players = dbContext.Players;
        _dbContext = dbContext;
    }
    public async Task AddAsync(Player player)
    {
        await _players.AddAsync(player);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Player> GetByIdAsync(Guid playerId)
        => _players.SingleOrDefaultAsync(p => p.PlayerId == playerId);
}