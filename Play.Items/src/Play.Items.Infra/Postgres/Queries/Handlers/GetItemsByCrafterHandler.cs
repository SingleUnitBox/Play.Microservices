using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

public class GetItemsByCrafterHandler : IQueryHandler<GetItemsByCrafter, IEnumerable<ItemDto>>
{
    private readonly ItemsPostgresDbContext _dbContext;

    public GetItemsByCrafterHandler(ItemsPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ItemDto>> QueryAsync(GetItemsByCrafter query)
    {
        var items = await _dbContext.Items
            .AsNoTracking()
            .Where(i => i.Crafter.CrafterId == query.CrafterId)
            .ToListAsync();

        return items is null
            ? null
            : items.Select(i => i.AsDto());
    }
}