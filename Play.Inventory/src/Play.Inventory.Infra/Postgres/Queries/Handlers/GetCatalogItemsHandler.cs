using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Postgres.Queries.Handlers;

public class GetCatalogItemsHandler : IQueryHandler<GetCatalogItems, IEnumerable<CatalogItemDto>>
{
    private readonly DbSet<CatalogItem> _catalogItems;

    public GetCatalogItemsHandler(InventoryPostgresDbContext dbContext)
    {
        _catalogItems = dbContext.CatalogItems;
    }
    
    public async Task<IEnumerable<CatalogItemDto>> QueryAsync(GetCatalogItems query)
    {
        var items = await _catalogItems
            .AsNoTracking()
            .ToListAsync();

        return items.Select(i => i.AsDto()).ToList();
    }
}