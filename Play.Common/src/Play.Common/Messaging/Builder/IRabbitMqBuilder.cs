using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Messaging.Builder;

public interface IRabbitMqBuilder
{
    IServiceCollection Services { get; }
    IServiceCollection Build();
}