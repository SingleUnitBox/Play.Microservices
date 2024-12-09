using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class EmptyDescriptionException : PlayException
{
    public EmptyDescriptionException()
        : base($"Description cannot be empty.")
    {
    }
}