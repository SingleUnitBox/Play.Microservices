using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events.External.Items;

public record ItemDeleted(Guid ItemId) : IEvent;