using System.Data;
using Microsoft.EntityFrameworkCore;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Infra.Queries;
using Play.Inventory.Infra.Queries.Handlers;

namespace Play.Inventory.Infra.Postgres.Queries;

public class PostgresHandlerDataAccessLayer : IDataAccessLayer
{
    private readonly InventoryPostgresDbContext _dbContext;

    public PostgresHandlerDataAccessLayer(InventoryPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<CatalogItem>> BrowseItems()
    {
        var items = await _dbContext.CatalogItems
            .AsNoTracking()
            .ToListAsync();
        return items;
    }
}