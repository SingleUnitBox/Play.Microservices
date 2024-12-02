using System.Security.Claims;

namespace Play.Common.Context;

public class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Role { get; }
    public Dictionary<string, IEnumerable<string>> Claims { get; }

    public IdentityContext(ClaimsPrincipal principal)
    {
        IsAuthenticated = principal.Identity.IsAuthenticated;
        UserId = IsAuthenticated ? Guid.Parse(principal.Identity.Name) : Guid.Empty;
        Role = IsAuthenticated ? principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value : string.Empty;
        Claims = IsAuthenticated ? principal.Claims.GroupBy(c => c.Type)
            .ToDictionary(
                k => k.Key,
                v => v.Select(c => c.Value))
            : new Dictionary<string, IEnumerable<string>>();
    }
}