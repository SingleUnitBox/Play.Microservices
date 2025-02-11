using Microsoft.EntityFrameworkCore;
using Play.Common.Abs.Queries;
using Play.Items.Application.DTO;
using Play.Items.Application.Queries;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Queries.Handlers;

public class GetCrafterHandler : IQueryHandler<GetCrafter, CrafterDto>
{
    private readonly ItemsPostgresDbContext _dbContext;
    private readonly DbSet<Crafter> _crafters;

    public GetCrafterHandler(ItemsPostgresDbContext dbContext)
    {
        _dbContext = dbContext;
        _crafters = dbContext.Crafters;
    }
    
    public async Task<CrafterDto> QueryAsync(GetCrafter query)
    {
        var crafter = await _crafters
            .AsNoTracking()
            .Include(c => c.Skills)
            .Include(c => c.Items)
            .SingleOrDefaultAsync(c => c.CrafterId == query.CrafterId);
        
        return crafter is null
            ? null
            : new CrafterDto
            {
                CrafterId = crafter.CrafterId,
                CrafterName = crafter.Name,
                Skills = crafter.Skills.Select(s => s.SkillName.Value),
            };
    }
}