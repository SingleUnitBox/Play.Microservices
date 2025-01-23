using Play.Common.Abs.Events;

namespace Play.User.Core.Events;

public record UserCreated(Guid UserId, string Username) : IEvent;