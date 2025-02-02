using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

public class GetItemsHandler : IQueryHandler<GetItems, IEnumerable<ItemDto>>
{
    private readonly DbSet<Item> _items;

    public GetItemsHandler(ItemsPostgresDbContext dbContext)
    {
        _items = dbContext.Items;
    }
    
    public async Task<IEnumerable<ItemDto>> QueryAsync(GetItems query)
    {
        var items = await _items
            .AsNoTracking()
            .ToListAsync();

        return items.Select(i => i.AsDto());
    }
}