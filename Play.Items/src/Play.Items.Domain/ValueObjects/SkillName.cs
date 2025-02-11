using Play.Items.Domain.Exceptions;

namespace Play.Items.Domain.ValueObjects;

public class SkillName
{
    public string Value { get; }

    public SkillName(string skillName)
    {
        if (string.IsNullOrWhiteSpace(skillName))
        {
            throw new InvalidSkillNameException();
        }
        
        Value = skillName;
    }
    
    public static implicit operator SkillName(string value) => new(value);
    public static implicit operator string(SkillName value) => value.Value;
}