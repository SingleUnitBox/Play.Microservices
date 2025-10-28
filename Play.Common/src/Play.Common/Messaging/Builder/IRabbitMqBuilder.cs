using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Messaging.Builder;

public interface IRabbitMqBuilder
{
    IServiceCollection Services { get; }
    IConfiguration  Configuration { get; }
    IServiceCollection Build();
}