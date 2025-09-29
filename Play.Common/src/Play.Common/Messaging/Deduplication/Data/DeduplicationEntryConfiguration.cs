using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Play.Common.Messaging.Deduplication.Data;

internal sealed class DeduplicationEntryConfiguration : IEntityTypeConfiguration<DeduplicationEntry>
{
    public void Configure(EntityTypeBuilder<DeduplicationEntry> builder)
    {
        builder.ToTable("DeduplicationEntries");
        builder.HasKey(d => d.MessageId);
    }
}