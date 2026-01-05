using Play.World.Domain.Entities;
using Play.World.Domain.ValueObjects;

namespace Play.World.Domain.Repositories;

public interface IItemLocationsRepository
{
    Task AddAsync(ItemLocation location);
    
    Task<ItemLocation> GetByItemIdAsync(Guid itemId);
    
    Task<IEnumerable<ItemLocation>> GetNearbyAsync(Coordinate center, int radiusMeters);
    
    // Task<IEnumerable<ItemLocation>> GetInZoneAsync(Guid zoneId);
}