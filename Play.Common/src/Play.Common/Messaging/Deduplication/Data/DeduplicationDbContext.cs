using Microsoft.EntityFrameworkCore;

namespace Play.Common.Messaging.Deduplication.Data;

internal sealed class DeduplicationDbContext(DbContextOptions<DeduplicationDbContext> options) : DbContext(options)
{
    public DbSet<DeduplicationEntry> DeduplicationEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("deduplication");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}