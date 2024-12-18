namespace Play.User.Contracts.Events;

public record UsernameChanged(Guid UserId, string Username);