using Play.Common.Abs.Events;

namespace Play.Inventory.Application.Events.External.Users;

public record UserCreated(Guid UserId, string Username) : IEvent;