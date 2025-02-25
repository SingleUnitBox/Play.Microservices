using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Play.Common.Consul.Services;

public class ConsulHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConsulHostedService> _logger;

    public ConsulHostedService(IServiceProvider serviceProvider,
        ILogger<ConsulHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var consulService = scope.ServiceProvider.GetRequiredService<IConsulService>();
        var serviceRegistration = scope.ServiceProvider.GetRequiredService<ServiceRegistration>();
        var response = await consulService.RegisterServiceAsync(serviceRegistration);
        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"Registered a service with id '{serviceRegistration.Id}' in Consul");
            return;
        }
        
        _logger.LogError($"There was an error registering a service with id '{serviceRegistration.Id}' in Consul");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}