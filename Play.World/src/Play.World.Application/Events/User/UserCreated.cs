using Play.Common.Abs.Events;

namespace Play.World.Application.Events.User;

public record UserCreated(Guid UserId, string Username) : IEvent;