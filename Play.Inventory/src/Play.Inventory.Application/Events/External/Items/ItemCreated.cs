using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Inventory.Application.Events.External.Items;

public record ItemCreated(Guid ItemId, string Name, decimal Price, int Version) : IEvent, IVersionedMessage
{
}