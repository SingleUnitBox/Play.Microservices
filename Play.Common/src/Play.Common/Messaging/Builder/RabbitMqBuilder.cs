using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Messaging.Builder;

public class RabbitMqBuilder(IServiceCollection services, IConfiguration configuration) : IRabbitMqBuilder
{
    public IServiceCollection Services { get; } = services;

    public IConfiguration Configuration { get; } = configuration;
    
    public IServiceCollection Build()
        => Services;
}