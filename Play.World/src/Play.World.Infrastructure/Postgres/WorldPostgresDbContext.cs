using Microsoft.EntityFrameworkCore;
using Play.World.Domain.Entities;

namespace Play.World.Infrastructure.Postgres;

public class WorldPostgresDbContext : DbContext
{
    public DbSet<ItemLocation> ItemLocations { get; set; }

    public WorldPostgresDbContext(DbContextOptions<WorldPostgresDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("play.world");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}