using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Skill
{
    public SkillId SkillId { get; }
    public SkillName SkillName { get; }

    private Skill()
    {
        
    }
    
    private Skill(string skillName)
    {
        SkillId = Guid.NewGuid();
        SkillName = skillName;
    }
    
    public static Skill Create(string skillName)
        => new Skill(skillName);
}