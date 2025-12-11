using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Infra.Postgres.Configuration;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.Property(c => c.DeletedAt)
            .IsRequired(false);
    }
}