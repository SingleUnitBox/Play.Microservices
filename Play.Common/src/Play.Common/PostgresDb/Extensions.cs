using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.PostgresDb;

public static class Extensions
{
    public static IServiceCollection AddPostgresDb<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        using var serviceProvider = services.BuildServiceProvider();
        var postgresOptions = serviceProvider
            .GetRequiredService<IConfiguration>()
            .GetSection(nameof(PostgresSettings))
            .Get<PostgresSettings>();
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(postgresOptions.ConnectionString);
        });
        
        return services;
    }
}