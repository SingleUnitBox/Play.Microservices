using Play.Common.Temp.Exceptions;

namespace Play.Catalog.Domain.Exceptions;

public class EmptyNameException : PlayException
{
    public EmptyNameException()
        : base($"Name cannot be empty.")
    {
    }
}