using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

public class GetElementsHandler(ItemsPostgresDbContext dbContext) : IQueryHandler<GetElements, IEnumerable<ElementDto>>
{
    public async Task<IEnumerable<ElementDto>> QueryAsync(GetElements query)
    {
        var elements = await dbContext.Elements
            .AsNoTracking()
            .ToListAsync();

        return elements.Select(e => new ElementDto
        {
            ElementId = e.ElementId,
            Name = e.ElementName.Value,
        });
    }
}