using Play.Common.Abs.Exceptions;

namespace Play.World.Application.Exceptions;

public class PlayerAlreadyExistException : PlayException
{
    public Guid PlayerId { get; }
    
    public PlayerAlreadyExistException(Guid playerId)
        : base($"Player with id '{playerId}' already exist.")
    {
        PlayerId = playerId;
    }
}