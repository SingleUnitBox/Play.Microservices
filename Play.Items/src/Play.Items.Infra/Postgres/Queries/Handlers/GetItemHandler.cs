using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

public class GetItemHandler : IQueryHandler<GetItem, ItemDto>
{
    private readonly DbSet<Item> _items;
    private readonly ItemsPostgresDbContext _dbContext;

    public GetItemHandler(ItemsPostgresDbContext dbContext)
    {
        _items = dbContext.Items;
        _dbContext = dbContext;
    }
    
    public async Task<ItemDto> QueryAsync(GetItem query)
    {
        // var item = await _items
        //     .AsNoTracking()
        //     .Include(i => i.Crafter)
        //     .SingleOrDefaultAsync(i => i.Id == query.ItemId);
        var item = await _items
            .FromSqlInterpolated($"SELECT * FROM \"play.items\".\"Items\" WHERE \"Id\" = {query.ItemId}")
            .SingleOrDefaultAsync();

        return item is null
            ? null
            : new ItemDto
            (
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.CreatedDate
            );
    }
}