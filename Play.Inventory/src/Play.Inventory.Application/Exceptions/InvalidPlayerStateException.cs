using Play.Common.Abs.Exceptions;

namespace Play.Inventory.Application.Exceptions;

public class InvalidPlayerStateException : PlayException
{
    public Guid PlayerID { get; }
    public string PlayerState { get; }
    public InvalidPlayerStateException(Guid playerId, string playerState)
        : base($"Player with id '{playerId}' state '{playerState}' is invalid.")
    {
        PlayerID = playerId;
        PlayerState = playerState;
    }
}