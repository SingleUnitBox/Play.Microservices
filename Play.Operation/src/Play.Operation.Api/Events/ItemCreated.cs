using Play.Common.Abs.Events;

namespace Play.Operation.Api.Events;

public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent;