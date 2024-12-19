namespace Play.Common.Abs.Auth;

public interface IJwtManager
{
    JwtToken GenerateToken(string userId, string role, Dictionary<string, IEnumerable<string>> claims);
}