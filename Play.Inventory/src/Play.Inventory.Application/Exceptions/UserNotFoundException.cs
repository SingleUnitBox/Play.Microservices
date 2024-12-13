using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class UserNotFoundException : PlayException
{
    public Guid UserId { get; }
    public UserNotFoundException(Guid userId)
        : base($"User with id '{userId}' was not found.")
    {
        UserId = userId;
    }
}