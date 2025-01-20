using Play.Common.Abs.Events;

namespace Play.Items.Application.Events;

public record ItemDeleted(Guid ItemId) : IEvent;