using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;

namespace Play.Inventory.Infra.Postgres.Queries;

public class GetCatalogItemsHandler //: IQueryHandler<GetCatalogItems, IReadOnlyCollection<ItemDto>>
{
    private readonly InventoryPostgresDbContext _dbContext;

    public GetCatalogItemsHandler(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<ItemDto>> QueryAsync(GetCatalogItems query)
    {
        var items = await _dbContext.CatalogItems
            .AsNoTracking()
            .ToListAsync();

        return items.Select(i => i.AsDto()).ToList();
    }
}