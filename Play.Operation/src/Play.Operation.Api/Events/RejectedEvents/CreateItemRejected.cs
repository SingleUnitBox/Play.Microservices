using Play.Common.Abs.Events;

namespace Play.Operation.Api.Events.RejectedEvents;

public record CreateItemRejected(Guid ItemId, string Name, string Reason, string Code) : IRejectedEvent;