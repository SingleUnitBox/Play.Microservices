using Play.Common.Abs.Events;

namespace Play.Items.Contracts.Events;

public record ItemDeleted(Guid ItemId) : IEvent;