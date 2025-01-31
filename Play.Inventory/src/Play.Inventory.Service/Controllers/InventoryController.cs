using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Context;
using Play.Common.Controllers;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;

namespace Play.Inventory.Service.Controllers;

[Authorize]
public class InventoryController : BaseController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IContext _context;
    private readonly IQueryDispatcher _queryDispatcher;

    public InventoryController(ICommandDispatcher commandDispatcher,
        IContext context,
        IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _context = context;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync()
    {
        return Ok(await _queryDispatcher
            .QueryAsync(new GetPlayerInventoryItems(_context.IdentityContext.UserId)));
    }
}