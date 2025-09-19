using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Play.Common.Abs;
using Play.Common.Abs.RabbitMq;
using Play.Common.Metrics.Tracing;

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
                    // below line will add allow to add CustomSpans for AsyncMessaging
                    // .AddSource()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        // endpoint could be moved to settings Tracing:endpoint or Jaeger:endpoint
                        options.Endpoint = new Uri("http://localhost:4317");
                    });
            });
        playConfigurator.Services.TryDecorate<IBusPublisher, TracingBusPublisherDecorator>();
        
        return playConfigurator;
    }

    public static IApplicationBuilder UsePlayMetrics(this IApplicationBuilder app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        return app;
    }
}