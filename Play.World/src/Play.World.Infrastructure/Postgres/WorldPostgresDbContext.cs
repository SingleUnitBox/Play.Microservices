using Microsoft.EntityFrameworkCore;
using Play.World.Domain.Entities;
using Play.World.Domain.ValueObjects;

namespace Play.World.Infrastructure.Postgres;

public class WorldPostgresDbContext : DbContext
{
    public DbSet<ItemLocation> ItemLocations { get; set; }

    public DbSet<Zone> Zones { get; set; }

    public WorldPostgresDbContext(DbContextOptions<WorldPostgresDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("play.world");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        
        //ignore value objects so they are not treated as entities
        modelBuilder.Ignore<Coordinate>();
        modelBuilder.Ignore<ZoneBoundary>();
        // modelBuilder.Ignore<ZoneType>();
    }
}