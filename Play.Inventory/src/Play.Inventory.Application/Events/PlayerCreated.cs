using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events;

public record PlayerCreated(Guid PlayerId) : IEvent;