using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Play.User.Core.Auth;

public class JwtManager : IJwtManager
{
    private readonly AuthSettings _settings;
    private readonly string _issuer;
    private readonly SigningCredentials _credentials;
    
    public JwtManager(AuthSettings settings)
    {
        _settings = settings;
        _issuer = settings.Issuer;
        _credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.IssuerSigningKey)),
            SecurityAlgorithms.HmacSha256);
    }
    
    public JwtToken GenerateToken(string userId, string role,
        Dictionary<string, IEnumerable<string>> claims)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("User id claim (subject) cannot be empty", nameof(userId));
        }
        
        var now = DateTime.UtcNow;
        var jwtClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString())
        };
        if (!string.IsNullOrWhiteSpace(role))
        {
            jwtClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        if (claims?.Any() is true)
        {
            var customClaims = new List<Claim>();
            foreach (var (claim, values) in claims)
            {
                customClaims.AddRange(values.Select(v => new Claim(claim, v)));
            }

            jwtClaims.AddRange(customClaims);
        }

        var expires = now.Add(_settings.Expiry);
        var jwt = new JwtSecurityToken(
            _issuer,
            claims: jwtClaims,
            notBefore: now,
            expires: expires,
            signingCredentials: _credentials);
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JwtToken()
        {
            AccessToken = token,
            Expires = new DateTimeOffset(expires).ToUnixTimeMilliseconds(),
            Id = userId,
            Role = role ?? string.Empty,
            Claims = claims ?? new Dictionary<string, IEnumerable<string>>(),
        };
    }
}