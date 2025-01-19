using MassTransit.Topology;
using Play.Common.Abs.Events;

namespace Play.Items.Contracts.Events;

[EntityName("ItemCreatedNameFromAttribute")]
public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent;