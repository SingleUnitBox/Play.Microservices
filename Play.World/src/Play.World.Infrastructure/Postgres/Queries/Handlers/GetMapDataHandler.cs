using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class GetMapDataHandler(WorldPostgresDbContext dbContext) : IQueryHandler<GetMapData, MapDataDto>
{
    public async Task<MapDataDto> QueryAsync(GetMapData query)
    {
        var itemLocations = await dbContext.ItemLocations.ToListAsync();

        return new MapDataDto()
        {
            ItemLocations = itemLocations.Select(il => new ItemLocationDto()
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
            })
        };
    }
}