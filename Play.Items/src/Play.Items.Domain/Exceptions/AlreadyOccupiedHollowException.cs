using Play.Common.Abs.Exceptions;

namespace Play.Items.Domain.Exceptions;

public class AlreadyOccupiedHollowException : PlayException
{
    public AlreadyOccupiedHollowException() : base($"Hollow is already occupied. Cannot embed artifact.")
    {
    }
}