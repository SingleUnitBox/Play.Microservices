namespace Play.Common.Abs.Auth;

public class JwtPayload
{
    public string Subject { get; set; }

    public string Role { get; set; }

    public long Expires { get; set; }

    public IDictionary<string, IEnumerable<string>> Claims { get; set; }
}