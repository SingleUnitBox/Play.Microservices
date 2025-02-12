using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Entities;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Infra.Postgres.Configuration;

public class ElementConfiguration : IEntityTypeConfiguration<Element>
{
    public void Configure(EntityTypeBuilder<Element> builder)
    {
        builder.HasKey(e => e.ElementId);
        builder.Property(e => e.ElementId)
            .HasConversion(e => e.Value, value => new ElementId(value));
        builder.Property(e => e.ElementName)
            .HasConversion(e => e.Value, value => new(value));

        builder.HasData(
            Element.Create("Earth"),
            Element.Create("Wind"),
            Element.Create("Water"),
            Element.Create("Fire")
            );
    }
}