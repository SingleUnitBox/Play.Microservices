using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.RabbitMq.Builder;

public interface IRabbitMqBuilder
{
    IRabbitMqBuilder AddEventConsumer();
    IRabbitMqBuilder AddCommandConsumer();
    IServiceCollection Build();
}