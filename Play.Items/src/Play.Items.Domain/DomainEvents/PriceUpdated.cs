using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Abs.SharedKernel.Types;

namespace Play.Items.Domain.DomainEvents;

public record PriceUpdated(AggregateRootId ItemId, string Name, decimal Price) : IDomainEvent;