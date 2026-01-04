using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

internal sealed class GetCraftersHandler(ItemsPostgresDbContext dbContext) : IQueryHandler<GetCrafters, IEnumerable<CrafterDto>>
{
    public async Task<IEnumerable<CrafterDto>> QueryAsync(GetCrafters query)
    {
        var crafters = await dbContext.Crafters
            .AsNoTracking()
            .ToListAsync();

        return crafters.Select(c => new CrafterDto()
        {
            CrafterId = c.CrafterId,
            CrafterName = c.Name,
        });
    }
}