using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class EmptyNameException : PlayException
{
    public EmptyNameException()
        : base($"Name cannot be empty.")
    {
    }
}