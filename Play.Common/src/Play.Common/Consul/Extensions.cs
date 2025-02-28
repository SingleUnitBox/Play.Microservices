using Microsoft.Extensions.DependencyInjection;
using Play.Common.Common;
using Play.Common.Consul.Builders;
using Play.Common.Consul.Http;
using Play.Common.Consul.MessageHandlers;
using Play.Common.Consul.Models;
using Play.Common.Consul.Services;
using Play.Common.Http;
using Play.Common.Settings;

namespace Play.Common.Consul;

public static class Extensions
{
    public static IConsulBuilder AddConsul(this IServiceCollection services)
    {
        var builder = new ConsulBuilder(services);
        services.AddSingleton(builder);
        var consulSettings = services.GetSettings<ConsulSettings>(nameof(ConsulSettings));
        var httpClientSettings = services.GetSettings<HttpClientSettings>(nameof(HttpClientSettings));

        return builder.AddConsul(consulSettings, httpClientSettings);
    }

    public static IConsulBuilder AddConsul(this IConsulBuilder builder,
        ConsulSettings consulSettings, HttpClientSettings httpClientSettings)
    {
        builder.Services.AddSingleton(consulSettings);
        if (httpClientSettings.Type?.ToLowerInvariant() == "consul")
        {
            builder.Services.AddTransient<ConsulServiceDiscoveryMessageHandler>();
            builder.Services.AddHttpClient<IHttpClient, ConsulHttpClient>()
                .AddHttpMessageHandler<ConsulServiceDiscoveryMessageHandler>();
            builder.Services.RemoveHttpClient();
            builder.Services.AddHttpClient<IConsulHttpClient, ConsulHttpClient>()
                .AddHttpMessageHandler<ConsulServiceDiscoveryMessageHandler>();
            builder.Services.AddSingleton<IServiceId, ServiceId>();
            builder.Services.AddHostedService<ConsulHostedService>();
            builder.Services.AddTransient<IConsulServicesRegistry, ConsulServiceRegistry>();
            var registration = builder.Services.CreateConsulAgentRegistration(consulSettings);
            if (registration is null)
            {
                return builder;
            }
        
            builder.Services.AddSingleton(registration);
        }
        
        return builder;
    }

    private static ServiceRegistration CreateConsulAgentRegistration(this IServiceCollection services,
        ConsulSettings consulSettings)
    {
        var enabled = consulSettings.Enabled;
        if (!enabled)
        {
            return null;
        }

        services.AddHttpClient<IConsulService, ConsulService>(c => c.BaseAddress = new Uri(consulSettings.Url));

        string serviceId;
        using (var serviceProvider = services.BuildServiceProvider())
        {
            serviceId = serviceProvider.GetRequiredService<IServiceId>().Id;
        }

        var registration = new ServiceRegistration
        {
            Id = $"{consulSettings.Service}:{serviceId}",
            Name = consulSettings.Service,
            Address = consulSettings.Address,
            Port = consulSettings.Port,
        };
        
        return registration;
    }
    
    public static void RemoveHttpClient(this IServiceCollection services)
    {
        var registryType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => t.Name == "HttpClientMappingRegistry");
        var registry = services.SingleOrDefault(s => s.ServiceType == registryType)?.ImplementationInstance;
        var registrations = registry?.GetType().GetProperty("TypedClientRegistrations");
        var clientRegistrations = registrations?.GetValue(registry) as IDictionary<Type, string>;
        clientRegistrations?.Remove(typeof(IHttpClient));
    }
}