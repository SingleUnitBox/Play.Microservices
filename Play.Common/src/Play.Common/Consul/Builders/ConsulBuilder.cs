using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Consul.Builders;

public class ConsulBuilder : IConsulBuilder
{
    private readonly IServiceCollection _services;
    
    public IServiceCollection Services => _services;

    public ConsulBuilder(IServiceCollection services)
    {
        _services = services;
    }
    
    public IServiceCollection Build()
        => _services;
}