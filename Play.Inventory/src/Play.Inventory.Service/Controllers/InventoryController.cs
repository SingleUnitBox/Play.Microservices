using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Common.Abs.Commands;
using Play.Common.Context;
using Play.Common.Controllers;
using Play.Inventory.Application;
using Play.Inventory.Application.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Service.Controllers;

public class InventoryController : BaseController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IContext _context;

    public InventoryController(ICommandDispatcher commandDispatcher,
        IContext context)
    {
        _commandDispatcher = commandDispatcher;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync()
    {
        return Ok();
    }

    //[Authorize]
    [HttpPost]
    public async Task<ActionResult> PostAsync(PurchaseItem command)
    {
        await _commandDispatcher.DispatchAsync(command with { UserId = _context.IdentityContext.UserId });
        return NoContent();
    }
}