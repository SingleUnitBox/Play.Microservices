using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class PlayerNotFoundException : PlayException
{
    public Guid PlayerId { get; }
    public PlayerNotFoundException(Guid playerId)
        : base($"Player with id '{playerId}' was not found.")
    {
        PlayerId = playerId;
    }
}