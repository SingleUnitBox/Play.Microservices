using Play.World.Domain.Entities;

namespace Play.World.Domain.Repositories;

public interface IPlayerRepository
{
    Task AddAsync(Player player);
    
    Task<Player> GetByIdAsync(Guid playerId);
}