using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Items.Application.Events;

public record SocketMade(Guid ItemId, string Socket, int Version) : IEvent, IVersionedMessage;