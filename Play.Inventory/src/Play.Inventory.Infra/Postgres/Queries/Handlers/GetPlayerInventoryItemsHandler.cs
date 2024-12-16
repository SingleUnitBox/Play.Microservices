using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Inventory.Application.DTO;
using Play.Inventory.Application.Queries;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Postgres.Queries.Handlers;

public class GetPlayerInventoryItemsHandler : IQueryHandler<GetPlayerInventoryItems, IReadOnlyCollection<InventoryItemDto>>
{
    private readonly DbSet<CatalogItem> _catalogItems;
    private readonly DbSet<InventoryItem> _inventoryItems;
    

    public GetPlayerInventoryItemsHandler(InventoryPostgresDbContext dbContext)
    {
        _catalogItems = dbContext.CatalogItems;
        _inventoryItems = dbContext.InventoryItems;
    }
    
    public async Task<IReadOnlyCollection<InventoryItemDto>> QueryAsync(GetPlayerInventoryItems query)
    {
        var inventoryItems = await _inventoryItems
            .AsNoTracking()
            .Where(i => i.PlayerId == query.PlayerId)
            .ToListAsync();

        var catalogItemIds = inventoryItems.Select(i => i.CatalogItemId).ToList();
        var catalogItems = await _catalogItems
            .AsNoTracking()
            .Where(i => catalogItemIds.Contains(i.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name);

        var items = inventoryItems.Select(i => new InventoryItemDto
        {
            Name = catalogItems[i.CatalogItemId],
            Quantity = i.Quantity,
        });

        return items.ToList();
    }
}