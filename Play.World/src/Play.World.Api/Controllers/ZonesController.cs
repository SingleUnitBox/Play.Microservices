using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Api.Controllers;

public class ZonesController(IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ZoneDto>>> GetZoneAsync()
    {
        var zones = await queryDispatcher.QueryAsync(new GetZones());
        return Ok(zones);
    }
}