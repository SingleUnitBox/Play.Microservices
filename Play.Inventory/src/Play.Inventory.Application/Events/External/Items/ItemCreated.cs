using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events.External.Items;

public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent
{
    public int Version => 1;
}