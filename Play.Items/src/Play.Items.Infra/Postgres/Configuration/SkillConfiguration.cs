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
            Skill.Create("Forging"),
            Skill.Create("Mixing"),
            Skill.Create("Weaving"),
            Skill.Create("Griding")
        );
    }
}