using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using Play.Common.Abs;

namespace Play.Common.Metrics;

public static class Extensions
{
    public static IPlayConfigurator AddPlayMetrics(this IPlayConfigurator playConfigurator,
        string[]? meterNames = null)
    {
        playConfigurator.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddPrometheusExporter();

                if (meterNames != null && meterNames.Any())
                {
                    foreach (var meter in meterNames)
                        metrics.AddMeter(meter);
                }
            }); 
        
        return playConfigurator;
    }

    public static IApplicationBuilder UsePlayMetrics(this IApplicationBuilder app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        return app;
    }
}