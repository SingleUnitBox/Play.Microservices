using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Play.Common.Messaging.Outbox.Data;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };
    
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.SerializedMessage).IsRequired();
        builder.Property(o => o.Headers)
            .HasConversion(h => JsonSerializer.Serialize(h, SerializerOptions),
                h => JsonSerializer.Deserialize<IDictionary<string, object>>(h, SerializerOptions),
                new ValueComparer<IDictionary<string, object>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary(x => x.Key, x => x.Value)));
        builder.Property(o => o.StoredAt).IsRequired();
        builder.Ignore(o => o.Message);
    }
}