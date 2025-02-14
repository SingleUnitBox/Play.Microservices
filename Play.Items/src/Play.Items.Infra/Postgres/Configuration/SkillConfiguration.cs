using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Play.Items.Domain.Entities;

namespace Play.Items.Infra.Postgres.Configuration;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasKey(s => s.SkillId);
        builder.Property(s => s.SkillId)
            .HasConversion(s => s.Value, value => new(value));
        builder.Property(s => s.SkillName)
            .HasConversion(s => s.Value, value => new(value));

        builder.HasData(
            Skill.Create(Guid.Parse("4b392707-748b-4f62-9545-e93feb1827cf"), "Forging"),
            Skill.Create(Guid.Parse("5eb6e794-28a0-47ad-bec8-d4ce7612f7c2"),"Mixing"),
            Skill.Create(Guid.Parse("e9aed6b8-92b6-4ffe-8dcd-25287e553b4d"),"Weaving"),
            Skill.Create(Guid.Parse("fc10a749-c4ae-4f6d-8966-4a51a1ac1af3"),"Griding")
        );
    }
}