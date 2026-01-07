using Play.Common.Abs.Exceptions;

namespace Play.World.Domain.Exceptions;

public class InvalidZoneBoundaryException : PlayException
{
    public InvalidZoneBoundaryException() : base("Polygon must have at least 3 points.")
    {
    }
}