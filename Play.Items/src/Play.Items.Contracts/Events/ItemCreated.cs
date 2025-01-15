using Play.Common.Abs.Events;

namespace Play.Items.Contracts.Events;

public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent;