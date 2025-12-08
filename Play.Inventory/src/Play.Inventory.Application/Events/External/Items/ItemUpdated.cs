using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events.External.Items;

public record ItemUpdated(Guid ItemId, string Name, decimal Price, int Version) : IEvent;