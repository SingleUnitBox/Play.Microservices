using MassTransit;
using Play.Common.Abs.Events;

namespace Play.Items.Contracts.Events;

//[EntityName("ItemCreatedNameFromAttribute")]
[MessageUrn("ItemCreated")]
public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent;