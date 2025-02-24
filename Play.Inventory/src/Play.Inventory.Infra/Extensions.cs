using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using Play.Inventory.Application.Services.Clients;
using Play.Inventory.Infra.Services.Clients;

namespace Play.Inventory.Infra;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var httpClientSettings = services.GetSettings<HttpClientSettings>(nameof(HttpClientSettings));
        services.AddSingleton(httpClientSettings);
        services.AddTransient<IUserServiceClient, UserServiceClient>();
        services.AddHttpClient();
        
        return services;
    }
}