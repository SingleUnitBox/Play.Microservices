using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Domain.Entities;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.DomainEvents;

public record SocketCreated(Item Item) : IDomainEvent;