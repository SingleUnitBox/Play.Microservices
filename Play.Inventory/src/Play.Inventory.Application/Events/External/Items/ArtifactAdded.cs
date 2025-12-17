using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Inventory.Application.Events.External.Items;

public record ArtifactAdded(Guid ItemId, string Artifact, int Version) : IEvent, IVersionedMessage;