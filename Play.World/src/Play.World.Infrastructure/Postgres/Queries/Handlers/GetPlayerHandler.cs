using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class GetPlayerHandler(WorldPostgresDbContext dbContext) : IQueryHandler<GetPlayer, PlayerDto>
{
    public async Task<PlayerDto> QueryAsync(GetPlayer query)
    {
        var player = await dbContext.Players
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.PlayerId == query.PlayerId);
        return player is not null
            ? new PlayerDto()
            {
                PlayerId = player.PlayerId,
                PlayerName = player.PlayerName,
                Position = new CoordinateDto()
                {
                    Longitude = player.Position.Longitude,
                    Latitude = player.Position.Latitude
                }
            }
            : null;
    }
}