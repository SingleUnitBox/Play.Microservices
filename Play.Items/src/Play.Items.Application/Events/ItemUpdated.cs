using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Items.Application.Events;

public record ItemUpdated(Guid ItemId, string Name, decimal Price) : IEvent, IVersionedMessage
{
    public int Version { get; }
}