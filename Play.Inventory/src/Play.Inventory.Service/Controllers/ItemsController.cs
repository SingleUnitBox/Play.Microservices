using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Common.Controllers;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Infra.Postgres;

namespace Play.Inventory.Service.Controllers;

public class ItemsController : BaseController
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly DbSet<CatalogItem> _catalogItems;

    public ItemsController(InventoryPostgresDbContext dbContext,
        IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _catalogItems = dbContext.CatalogItems;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CatalogItemDto>>> BrowseItems()
        => Ok(await _queryDispatcher.QueryAsync(new GetCatalogItems()));
}