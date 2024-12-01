namespace Play.User.Service.Context;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Role { get; }
    Dictionary<string, IEnumerable<string>> Claims { get; }
}