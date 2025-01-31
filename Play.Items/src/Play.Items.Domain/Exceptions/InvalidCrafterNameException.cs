using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class InvalidCrafterNameException : PlayException
{
    public InvalidCrafterNameException()
        : base($"Crafter name is invalid.")
    {
    }
}