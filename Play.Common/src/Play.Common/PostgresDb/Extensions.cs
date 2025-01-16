using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.PostgresDb.UnitOfWork;
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
        if (postgresOptions.Enabled)
        {
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(postgresOptions.ConnectionString);
            });
        }
        
        return services;
    }

    public static IServiceCollection AddPostgresUnitOfWork<TUnitOfWork, TImplementation>(this IServiceCollection services)
    where TUnitOfWork : IUnitOfWork where TImplementation : TUnitOfWork
    {
        services.AddScoped(typeof(TUnitOfWork), typeof(TImplementation));
        services.AddScoped(typeof(IUnitOfWork), typeof(TImplementation));
        
        return services;
    }
}