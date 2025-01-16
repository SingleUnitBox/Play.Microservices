using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Context;
using Play.Common.Controllers;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Contracts.Commands;

namespace Play.Inventory.Service.Controllers;

public class ItemsController(IQueryDispatcher queryDispatcher,
    ICommandDispatcher commandDispatcher,
    IContext context) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> BrowseItems()
        => Ok(await queryDispatcher.QueryAsync(new GetCatalogItems()));
    
    [HttpPost("{itemId:guid}/purchase")]
    public async Task<ActionResult> PostAsync(PurchaseItem command, Guid itemId)
    {
        await commandDispatcher.DispatchAsync(command with { ItemId = itemId, PlayerId = context.IdentityContext.UserId });
        return NoContent();
    }
}