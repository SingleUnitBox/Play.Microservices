using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Play.Common.Abs.Exceptions;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;
using Coordinate = Play.World.Domain.ValueObjects.Coordinate;

namespace Play.World.Infrastructure.Postgres.Repositories;

public class ItemLocationRepository : IItemLocationsRepository
{
    private readonly DbSet<ItemLocation> _itemLocations;
    private readonly WorldPostgresDbContext _dbContext;
    
    public ItemLocationRepository(WorldPostgresDbContext dbContext)
    {
        _itemLocations = dbContext.ItemLocations;
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(ItemLocation location)
    {
        await _itemLocations.AddAsync(location);
        await _dbContext.SaveChangesAsync();
    }

    public Task<ItemLocation> GetByItemIdAsync(Guid itemId)
        => _itemLocations.SingleOrDefaultAsync(x => x.ItemId == itemId);

    public async Task<IEnumerable<ItemLocation>> GetNearbyAsync(Coordinate center, int radiusMeters)
    {
        return await _itemLocations
            .FromSqlInterpolated($@"
                SELECT * FROM ""play.world"".""item_locations""
                WHERE is_collected = false
                AND ST_DWithin(
                    position::geography,
                    ST_SetSRID(ST_MakePoint({{center.Longitude}}, {{center.Latitude}}), 4326)::geography,
                    {radiusMeters}
                )
            ")
            .AsNoTracking()
            .ToListAsync();
    }

    // public Task<IEnumerable<ItemLocation>> GetInZoneAsync(Guid zoneId)
    // {
    //     throw new NotImplementedException();
    // }
}