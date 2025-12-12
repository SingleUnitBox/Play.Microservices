using Play.Common.Abs.Exceptions;

namespace Play.Items.Application.Exceptions;

public class InvalidHollowTypeException : PlayException
{
    public string HollowType { get; }
    public InvalidHollowTypeException(string hollowType) : base($"Hollow type of '{hollowType}' for socket is invalid.")
    {
        HollowType = hollowType;
    }
}