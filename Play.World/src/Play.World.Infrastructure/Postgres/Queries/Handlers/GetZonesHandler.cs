using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class GetZonesHandler(WorldPostgresDbContext dbContext) : IQueryHandler<GetZones, IEnumerable<ZoneDto>>
{
    public async Task<IEnumerable<ZoneDto>> QueryAsync(GetZones query)
    {
        var zones = await dbContext.Zones
            .ToListAsync();

        return zones.Select(z => new ZoneDto
        {
            ZoneId = z.Id,
            Name = z.Name,
            Boundary = new ZoneBoundaryDto
            {
                Points = z.Boundary.Points
                    .Select(p => new CoordinateDto()
                    {
                        Longitude = p.Longitude,
                        Latitude = p.Latitude,
                    })
                    .ToList()
            },
            Type = z.Type.Value
        });
    }
}