using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.RabbitMq.Builder;

public interface IRabbitMqBuilder
{
    IServiceCollection Services { get; }
    IServiceCollection Build();
}