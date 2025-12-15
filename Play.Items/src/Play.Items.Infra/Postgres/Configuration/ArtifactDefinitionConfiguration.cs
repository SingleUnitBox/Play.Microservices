using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Types;
using Play.Items.Infra.Postgres.Factories;

namespace Play.Items.Infra.Postgres.Configuration;

public class ArtifactDefinitionConfiguration : IEntityTypeConfiguration<ArtifactDefinition>
{
    public void Configure(EntityTypeBuilder<ArtifactDefinition> builder)
    {
        builder.HasKey(ad => ad.Name);
        builder.Property(ad => ad.CompatibleHollowType)
            .HasConversion(ht => ht.ToString(), ht => Enum.Parse<HollowType>(ht));
        builder.Property(ad => ad.BaseStats)
            .HasConversion(
                stat => JsonSerializer.Serialize(stat, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<Dictionary<string, int>>(json, (JsonSerializerOptions?)null)
                ?? new Dictionary<string, int>());
    }
}