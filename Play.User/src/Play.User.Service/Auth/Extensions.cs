using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Play.Common.MongoDb;

namespace Play.User.Service.Auth;

public static class Extensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongo(configuration)
            .AddMongoRepository<Entities.User>("users");
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IJwtManager, JwtManager>();
        services.AddSingleton<IPasswordHasher<Entities.User>, PasswordHasher<Entities.User>>();
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

        services.AddSingleton(authSettings);
        services.AddSingleton(tokenValidationParameters);
        
        services.AddAuthorization();
        
        return services;
    }
}