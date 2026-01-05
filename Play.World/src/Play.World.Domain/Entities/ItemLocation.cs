using Play.Common.Abs.SharedKernel;
using Play.World.Domain.ValueObjects;

namespace Play.World.Domain.Entities;

public class ItemLocation : AggregateRoot
{
    public Guid ItemId { get; private set; }

    public string ItemName { get; private set; }

    public Coordinate Position { get; private set; }

    public bool IsCollected { get; private set; }

    public DateTimeOffset DroppedAt { get; private set; }

    private ItemLocation()
    {
        
    }
    
    public static ItemLocation Create(Guid itemId, string itemName, Coordinate position)
    {
        var location = new ItemLocation
        {
            ItemId = itemId,
            ItemName = itemName,
            Position = position,
            IsCollected = false,
            DroppedAt = DateTimeOffset.UtcNow
        };
        
        return location;
    }
}