using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class InvalidSkillNameException : PlayException
{
    public InvalidSkillNameException() :
        base($"Skill name is invalid.")
    {
    }
}