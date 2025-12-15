using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Items.Domain.Entities;

namespace Play.Items.Domain.DomainEvents;

public record ArtifactEmbedded(Item Item) : IDomainEvent;