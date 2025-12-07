using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.DomainEvents;

public record ItemDeleted(AggregateRootId ItemId) : IDomainEvent;