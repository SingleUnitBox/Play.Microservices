using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.World.Application.Events.Items;

public record ItemCreated(Guid ItemId, string Name, int Version) : IEvent, IVersionedMessage;