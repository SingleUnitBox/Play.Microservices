using Play.Common.Abs.Events;
using Play.Common.Abs.Messaging.Ordering;

namespace Play.Items.Application.Events;

public record ArtifactAdded(Guid ItemId, string Artifact, int Version) : IEvent, IVersionedMessage;