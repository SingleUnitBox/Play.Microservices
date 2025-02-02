using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Configuration;

public class CrafterConfiguration : IEntityTypeConfiguration<Crafter>
{
    public void Configure(EntityTypeBuilder<Crafter> builder)
    {
        builder.HasKey(c => c.CrafterId);
        builder.HasMany(c => c.Items)
            .WithOne(i => i.Crafter);
        builder.Property(c => c.CrafterId)
            .HasConversion(id => id.Value, value => new(value));
        builder.Property(c => c.Name)
            .HasConversion(c => c.Value, c => new(c));
    }
}