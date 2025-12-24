using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Inventory.Application.Events.External.Items;

public record SocketMade(Guid ItemId, int Version) : IEvent, IVersionedMessage;