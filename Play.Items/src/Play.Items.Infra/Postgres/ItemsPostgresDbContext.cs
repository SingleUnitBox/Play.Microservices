using Microsoft.EntityFrameworkCore;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres;

public class ItemsPostgresDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Crafter> Crafters { get; set; }
    public DbSet<Element> Elements { get; set; }

    public ItemsPostgresDbContext(DbContextOptions<ItemsPostgresDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.HasDefaultSchema("play.items");
    }
}