using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.ValueObjects;

public class SkillId : TypeId
{
    public SkillId(Guid value) : base(value)
    {
    }
    
    public static implicit operator SkillId(Guid skillId) => new(skillId);
}