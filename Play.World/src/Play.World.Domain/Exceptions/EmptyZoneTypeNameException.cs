using Play.Common.Abs.Exceptions;

namespace Play.World.Domain.Exceptions;

public class EmptyZoneTypeNameException : PlayException
{
    public EmptyZoneTypeNameException() : base($"Zone type cannot be empty.")
    {
    }
}