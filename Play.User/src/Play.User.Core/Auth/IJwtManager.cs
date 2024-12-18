namespace Play.User.Core.Auth;

public interface IJwtManager
{
    JwtToken GenerateToken(string userId, string role, Dictionary<string, IEnumerable<string>> claims);
}