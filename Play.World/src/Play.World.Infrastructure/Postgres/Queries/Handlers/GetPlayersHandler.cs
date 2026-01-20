using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Infrastructure.Postgres.Queries.Handlers;

public class GetPlayersHandler(WorldPostgresDbContext dbContext) : IQueryHandler<GetPlayers, IEnumerable<PlayerDto>>
{
    public async Task<IEnumerable<PlayerDto>> QueryAsync(GetPlayers query)
    {
        var players = await dbContext.Players
            .AsNoTracking()
            .ToListAsync();

        return players.Select(p => new PlayerDto()
        {
            PlayerId = p.PlayerId,
            PlayerName = p.PlayerName,
            Position = new CoordinateDto()
            {
                Longitude = p.Position.Longitude,
                Latitude = p.Position.Latitude
            }
        });
    }
}