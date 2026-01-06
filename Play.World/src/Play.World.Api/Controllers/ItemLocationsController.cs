using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.World.Application.DTO;
using Play.World.Application.Queries;

namespace Play.World.Api.Controllers
{
    public class ItemLocationsController : BaseController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ItemLocationsController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapDataDto>>> GetMapDataAsync()
        {
            var mapDataDto = await _queryDispatcher.QueryAsync(new GetMapData());
            return Ok(mapDataDto);
        }
    }
}