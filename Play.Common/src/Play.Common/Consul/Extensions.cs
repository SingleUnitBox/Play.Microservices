﻿using Microsoft.Extensions.DependencyInjection;
using Play.Common.Consul.Builders;
using Play.Common.Consul.Services;
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
        builder.Services.AddHostedService<ConsulHostedService>();
        var registration = builder.Services.CreateConsulAgentRegistration(consulSettings);
        if (registration is null)
        {
            return builder;
        }
        
        builder.Services.AddSingleton(registration);
        
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

        var registration = new ServiceRegistration
        {
            Id = $"{consulSettings.Service}:{Guid.NewGuid()}",
            Name = consulSettings.Service,
            Address = consulSettings.Address,
            Port = consulSettings.Port,
        };
        
        return registration;
    }
}