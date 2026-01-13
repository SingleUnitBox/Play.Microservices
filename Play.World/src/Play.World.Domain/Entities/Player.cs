using Play.Common.Abs.SharedKernel;
using Play.World.Domain.ValueObjects;

namespace Play.World.Domain.Entities;

public class Player : AggregateRoot
{
    public Guid PlayerId { get; private set; }
    
    public string PlayerName { get; private set; }
    
    public Coordinate Position { get; private set; }

    private Player()
    {
        Id = Guid.NewGuid();
    }

    private Player(Guid id)
    {
        Id = id;
    }

    private Player(Guid playerId, string playerName, Coordinate position)
        : this()
    {
        PlayerId = playerId;
        PlayerName = playerName;
        Position = position;
    }

    public static Player Create(Guid playerId, string playerName, Coordinate position)
    {
        var player = new Player(playerId, playerName, position);
        
        return player;
    }
}