using Microsoft.EntityFrameworkCore;

namespace Play.Common.Messaging.Outbox.Data;

internal sealed class OutboxDbContext(DbContextOptions<OutboxDbContext> options) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("outbox");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}