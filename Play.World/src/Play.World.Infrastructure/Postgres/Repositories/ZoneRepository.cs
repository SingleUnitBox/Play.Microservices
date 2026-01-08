using Microsoft.EntityFrameworkCore;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;

namespace Play.World.Infrastructure.Postgres.Repositories;

public class ZoneRepository : IZoneRepository
{
    private readonly DbSet<Zone> _zones;
    private readonly WorldPostgresDbContext _dbContext;

    public ZoneRepository(WorldPostgresDbContext dbContext)
    {
        _zones = dbContext.Zones;
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Zone zone)
    {
        await _zones.AddAsync(zone);
        await _dbContext.SaveChangesAsync();
    }

    public Task<Zone> GetByNameAsync(string zoneName)
    {
        return _zones.SingleOrDefaultAsync(z => z.Name == zoneName);
    }
}