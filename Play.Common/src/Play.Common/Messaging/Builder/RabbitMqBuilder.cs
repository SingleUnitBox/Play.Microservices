using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Messaging.Builder;

public class RabbitMqBuilder(IServiceCollection services) : IRabbitMqBuilder
{
    public IServiceCollection Services { get; } = services;
    
    public IServiceCollection Build()
        => Services;
}