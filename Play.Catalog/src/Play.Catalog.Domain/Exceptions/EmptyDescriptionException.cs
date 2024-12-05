using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Domain.Exceptions;

public class EmptyDescriptionException : PlayException
{
    public EmptyDescriptionException()
        : base($"Description cannot be empty.")
    {
    }
}