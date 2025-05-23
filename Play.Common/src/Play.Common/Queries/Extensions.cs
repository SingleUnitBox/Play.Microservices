﻿using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Queries;
using Play.Common.Logging.Attributes;

namespace Play.Common.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.Scan(a => a.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>))
                .WithoutAttribute<LoggingDecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}