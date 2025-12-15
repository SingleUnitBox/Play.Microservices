using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Types;

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

        builder.OwnsOne(i => i.Socket, socket =>
        {
            socket.Property(s => s.HollowType)
                .HasConversion(ht => ht.ToString(), ht => Enum.Parse<HollowType>(ht));
            socket.OwnsOne(s => s.Artifact, artifact =>
            {
                artifact.Property(a => a.CompatibleHollow)
                    .HasConversion(ht => ht.ToString(), value => Enum.Parse<HollowType>(value));
                artifact.Property(a => a.Stats)
                    .HasConversion(stat => JsonSerializer.Serialize(stat, (JsonSerializerOptions?)null),
                        stat => JsonSerializer.Deserialize<Dictionary<string, int>>(stat, (JsonSerializerOptions?)null)
                                ?? new Dictionary<string, int>());
            });
        });
    }
}