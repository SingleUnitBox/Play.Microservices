namespace Play.Common.Abs.Auth;

public class JwtToken
{
    public string AccessToken { get; set; }
    public long Expires { get; set; }
    public string Id { get; set; }
    public string Role { get; set; }
    public IDictionary<string, IEnumerable<string>> Claims { get; set; }
}