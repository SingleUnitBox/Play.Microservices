using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Configuration;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasOne(i => i.Crafter)
            .WithMany(c => c.Items);
        builder.Property(i => i.Id)
            .HasConversion(i => i.Value, value => new(value));
        builder.Property(i => i.Name)
            .HasConversion(n => n.Value, value => new(value));
        builder.Property(i => i.Description)
            .HasConversion(n => n.Value, value => new(value));
        builder.Property(i => i.Price)
            .HasConversion(n => n.Value, value => new(value));
        builder.HasOne(i => i.Element);
    }
}