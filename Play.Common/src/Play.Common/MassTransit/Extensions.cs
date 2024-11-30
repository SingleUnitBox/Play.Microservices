﻿using System.Reflection;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration,
        Assembly assembly)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(assembly);
            configure.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMqSettings = configuration
                    .GetSection(nameof(RabbitMqSettings))
                    .Get<RabbitMqSettings>();
                var serviceSettings = configuration
                    .GetSection(nameof(ServiceSettings))
                    .Get<ServiceSettings>();
                
                cfg.Host(rabbitMqSettings.Host);
                cfg.ConfigureEndpoints(ctx, 
                    new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                
            });

            services.AddMassTransitHostedService();
        });
        
        return services;
    }
}