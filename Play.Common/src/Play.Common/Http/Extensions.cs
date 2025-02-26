using Microsoft.Extensions.DependencyInjection;
using Play.Common.Http.Serializer;
using Play.Common.Settings;

namespace Play.Common.Http;

public static class Extensions
{
    public static IServiceCollection AddCommonHttpClient(this IServiceCollection services)
    {
        services.AddSingleton<IHttpClientSerializer, HttpClientSerializer>();
        var httpClientSettings = services.GetSettings<HttpClientSettings>(nameof(HttpClientSettings));
        services.AddSingleton(httpClientSettings);
        services.AddHttpClient<IHttpClient, CommonHttpClient>();
        
        return services;
    }
}