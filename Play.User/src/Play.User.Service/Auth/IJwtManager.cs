namespace Play.User.Service.Auth;

public interface IJwtManager
{
    JwtToken GenerateToken(string userId, string role, Dictionary<string, IEnumerable<string>> claims);
}