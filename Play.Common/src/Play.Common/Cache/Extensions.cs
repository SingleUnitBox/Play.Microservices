using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.Cache;

public static class Extensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisSettings = services.GetSettings<RedisSettings>(nameof(RedisSettings));
            redisOptions.Configuration = redisSettings.ConnectionString;
        });
        
        return services;
    }
}