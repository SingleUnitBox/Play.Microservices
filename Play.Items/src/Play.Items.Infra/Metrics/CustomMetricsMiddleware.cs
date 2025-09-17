using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Items.Infra.Metrics;

public class CustomMetricsMiddleware : IMiddleware
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IDictionary<string, Action<ItemsMetrics>> _metrics = new Dictionary<string, Action<ItemsMetrics>>
    {
        [GetKey("GET", "/items")] = Query(),
        [GetKey("POST", "/items")] = Command(),
    };

    
    public CustomMetricsMiddleware(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;
        if (_metrics.TryGetValue(GetKey(request.Method, request.Path), out var action))
        {
            using var scope = _scopeFactory.CreateScope();
            var metrics = scope.ServiceProvider.GetRequiredService<ItemsMetrics>();
            action(metrics);
        }
        
        return next(context);
    }

    private static string GetKey(string method, string path)
        => $"{method}:{path}";

    private static Action<ItemsMetrics> Query()
        => metrics => metrics.IncrementQuery();

    private static Action<ItemsMetrics> Command()
        => metrics => metrics.IncrementCommand();
}