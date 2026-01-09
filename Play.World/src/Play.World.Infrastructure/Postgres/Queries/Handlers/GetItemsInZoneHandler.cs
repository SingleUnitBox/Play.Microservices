using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.DTO;
using Play.World.Application.Queries;
using Play.World.Domain.Repositories;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class GetItemsInZoneHandler(
    IZoneRepository zoneRepository,
    WorldPostgresDbContext dbContext) : IQueryHandler<GetItemsInZone, IEnumerable<ItemLocationDto>>
{
    public async Task<IEnumerable<ItemLocationDto>> QueryAsync(GetItemsInZone query)
    {
        var zone = await zoneRepository.GetByIdAsync(query.ZoneId);
        if (zone is null)
        {
            return Enumerable.Empty<ItemLocationDto>();
        }
        
        var items = await dbContext.ItemLocations
            .FromSqlInterpolated($@"
                SELECT il.* 
                FROM ""play.world"".""ItemLocations"" il
                JOIN ""play.world"".""Zones"" z ON z.""Name"" = {zone.Name}
                WHERE ST_Contains(z.""Boundary"", il.""Position"")
                AND il.""IsCollected"" = false
            ")
            .ToListAsync();
        
        return items
            .Select(il => new ItemLocationDto()
            {
                ItemId = il.ItemId,
                ItemName = il.ItemName,
                Position = new CoordinateDto
                {
                    Longitude = il.Position.Longitude,
                    Latitude = il.Position.Latitude
                },
                IsCollected = il.IsCollected,
                DroppedAt = il.DroppedAt,
            });

    }
}