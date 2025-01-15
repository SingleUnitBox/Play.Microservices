using Play.Common.Abs.Events;

namespace Play.User.Contracts.Events;

public record UserCreated(Guid UserId, string Username) : IEvent;