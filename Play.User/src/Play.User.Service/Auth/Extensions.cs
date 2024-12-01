using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Play.User.Service.Auth;

public static class Extensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IJwtManager, JwtManager>();
        services.AddSingleton<IPasswordHasher<Entities.User>, PasswordHasher<Entities.User>>();
        var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();
        services.AddSingleton(authSettings);
        
        return services;
    }
}