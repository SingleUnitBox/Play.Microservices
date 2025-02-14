using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Skill
{
    public SkillId SkillId { get; }
    public SkillName SkillName { get; }

    private Skill()
    {
        
    }
    
    private Skill(Guid skillId, string skillName)
    {
        SkillId = skillId;
        SkillName = skillName;
    }
    
    public static Skill Create(Guid skillId, string skillName)
        => new Skill(skillId, skillName);
}