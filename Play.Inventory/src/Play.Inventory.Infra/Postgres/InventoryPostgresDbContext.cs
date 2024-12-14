using Microsoft.EntityFrameworkCore;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Postgres;

public class InventoryPostgresDbContext : DbContext
{
    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<MoneyBag> MoneyBags { get; set; }
    
    public InventoryPostgresDbContext(DbContextOptions<InventoryPostgresDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.HasDefaultSchema("play.inventory");
    }
}