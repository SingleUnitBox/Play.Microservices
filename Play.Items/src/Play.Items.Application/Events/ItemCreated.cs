using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Items.Application.Events;

public record ItemCreated(Guid ItemId, string Name, decimal Price, int Version) : IEvent, IVersionedMessage
{
}