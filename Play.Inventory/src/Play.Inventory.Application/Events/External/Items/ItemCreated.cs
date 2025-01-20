
using MassTransit;
using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events.External.Items;

//[EntityName("ItemCreatedNameFromAttribute")]
[MessageUrn("ItemCreated")]
public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent;