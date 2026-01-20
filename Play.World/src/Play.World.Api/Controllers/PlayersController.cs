using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Api.Controllers;

public class PlayersController(IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayersAsync()
    {
        var players = await queryDispatcher.QueryAsync(new GetPlayers());
        return Ok(players);
    }
}