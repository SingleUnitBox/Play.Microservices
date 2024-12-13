using Microsoft.AspNetCore.Mvc;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;

namespace Play.Inventory.Service.Controllers;

public class ItemController(IQueryDispatcher queryDispatcher) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ItemDto>>> BrowseItems()
        => Ok(await queryDispatcher.QueryAsync(new GetCatalogItems()));
}