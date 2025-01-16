using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events;

public record ItemPurchased(Guid PlayerId, Guid ItemId, int Quantity) : IEvent;