namespace Play.User.Core.Events;

public record UsernameChanged(Guid UserId, string Username);