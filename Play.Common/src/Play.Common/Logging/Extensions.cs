using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;
using Play.Common.Settings;
using Serilog;

namespace Play.Common.Logging;

public static class Extensions
{
    public static IServiceCollection AddLoggingCommandHandlerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        return services;
    }

    public static IServiceCollection AddLoggingCommandConsumerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IConsumer<>), typeof(LoggingCommandConsumerDecorator<>));
        return services;
    }

    public static IServiceCollection AddLoggingQueryHandlerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        return services;
    }

    public static IHostBuilder UseSerilogWithSeq(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((ctx, config) =>
        {
            var seqSettings = ctx.Configuration.GetSettings<SeqSettings>(nameof(SeqSettings));
            var serviceSettings = ctx.Configuration.GetSettings<ServiceSettings>(nameof(ServiceSettings));
            config.WriteTo.Console();
            if (seqSettings.Enabled)
            {
                config.WriteTo.Seq(seqSettings.Url, apiKey: seqSettings.ApiKey)
                    .Enrich.WithProperty("ServiceName", serviceSettings.ServiceName);
            }
        });
        
        return hostBuilder;
    }
}