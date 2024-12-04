using Play.Common.Abstractions.Exceptions;

namespace Play.Catalog.Domain.Exceptions;

public class EmptyNameException : PlayException
{
    public EmptyNameException()
        : base($"Name cannot be empty.")
    {
    }
}