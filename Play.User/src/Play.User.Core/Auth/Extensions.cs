using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.User.Core.Auth;

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