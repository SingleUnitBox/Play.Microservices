using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Play.Common.Settings;

namespace Play.Common.Auth;

public static class Extensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
        
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = authSettings.ValidIssuer,
            ValidateIssuer = authSettings.ValidateIssuer,
            ValidateAudience = authSettings.ValidateAudience,
            ValidateLifetime = authSettings.ValidateLifetime,
            ClockSkew = TimeSpan.Zero,
        };
        if (string.IsNullOrWhiteSpace(authSettings.IssuerSigningKey))
        {
            throw new ArgumentException("Missing issuer signing key.", nameof(authSettings.IssuerSigningKey));
        }
        
        var rawKey = Encoding.UTF8.GetBytes(authSettings.IssuerSigningKey);
        tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);
        
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = tokenValidationParameters;
        });
        
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthorization();
        
        return services;
    }
}