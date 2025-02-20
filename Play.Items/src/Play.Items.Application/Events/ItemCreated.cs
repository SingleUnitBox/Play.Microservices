using Play.Common.Abs.Events;

namespace Play.Items.Application.Events;

public record ItemCreated(Guid ItemId, string Name, decimal Price) : IEvent
{
}