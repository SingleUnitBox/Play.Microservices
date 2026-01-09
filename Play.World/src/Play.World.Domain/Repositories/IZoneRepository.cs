using Play.World.Domain.Entities;

namespace Play.World.Domain.Repositories;

public interface IZoneRepository
{
    Task AddAsync(Zone zone);
    
    Task<Zone> GetByNameAsync(string zoneName);
    
    Task<Zone> GetByIdAsync(Guid zoneId);
}