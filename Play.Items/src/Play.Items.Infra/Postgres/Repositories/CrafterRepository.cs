using Microsoft.EntityFrameworkCore;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Postgres.Repositories;

public class CrafterRepository : ICrafterRepository
{
    private readonly DbSet<Crafter> _crafters;

    public CrafterRepository(ItemsPostgresDbContext dbContext)
    {
        _crafters = dbContext.Crafters;
    }

    public async Task<Crafter> GetCrafterById(Guid crafterId)
    {
        var crafter = await _crafters.FromSqlInterpolated(
            $"SELECT * FROM \"play.items\".\"Crafters\" WHERE \"CrafterId\" = {crafterId}")
                .SingleOrDefaultAsync();
        
        return crafter;
    }
}