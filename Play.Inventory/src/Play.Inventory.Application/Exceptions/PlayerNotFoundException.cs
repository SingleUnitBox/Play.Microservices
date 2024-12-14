using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class PlayerNotFoundException : PlayException
{
    public Guid UserId { get; }
    public PlayerNotFoundException(Guid userId)
        : base($"User with id '{userId}' was not found.")
    {
        UserId = userId;
    }
}