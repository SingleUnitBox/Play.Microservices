using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
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

    public static IPlayConfigurator AddPlayTracing(this IPlayConfigurator playConfigurator,
        IWebHostEnvironment environment)
    {
        playConfigurator.Services.AddOpenTelemetry()
            .WithTracing(trace =>
            {
                trace.SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(environment.ApplicationName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://localhost:4317");
                    });
            });
        
        return playConfigurator;
    }

    public static IApplicationBuilder UsePlayMetrics(this IApplicationBuilder app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        return app;
    }
}