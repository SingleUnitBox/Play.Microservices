using Play.Common.Abs.SharedKernel.Types;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Repositories;

public interface IPlayerRepository
{
    Task CreateAsync(Player player);
    Task UpdateAsync(Player player);
    Task DeleteAsync(Guid playerId);
    Task<Player> GetByIdAsync(Guid playerId);
}