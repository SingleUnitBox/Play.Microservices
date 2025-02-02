using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.DomainEvents;

public record ItemCreated(AggregateRootId ItemId, Name Name, Price Price) : IDomainEvent;