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
        builder.HasMany(c => c.Skills)
            .WithMany();

        builder.HasData(
            Crafter.Create(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"), "Din Foo"),
            Crafter.Create(Guid.Parse("33364e25-6544-48bd-b87d-37760ee27911"), "Arrgond"),
            Crafter.Create(Guid.Parse("8ce6633f-c318-4017-acef-369b86fd981d"), "Bleatcher")
            );
    }
}