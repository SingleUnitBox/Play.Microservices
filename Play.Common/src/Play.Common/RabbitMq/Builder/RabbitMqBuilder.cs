using Microsoft.Extensions.DependencyInjection;
using Play.Common.RabbitMq.Consumers;

namespace Play.Common.RabbitMq.Builder;

public class RabbitMqBuilder : IRabbitMqBuilder
{
    private readonly IServiceCollection _services;

    public RabbitMqBuilder(IServiceCollection services)
    {
        _services = services;
    }
    
    public IRabbitMqBuilder AddEventConsumer()
    {
        _services.AddSingleton<IEventConsumer, EventConsumer>();
        _services.AddHostedService<EventConsumerService>();

        return this;
    }

    public IRabbitMqBuilder AddCommandConsumer()
    {
        _services.AddSingleton<ICommandConsumer, CommandConsumer>();
        _services.AddHostedService<CommandConsumerService>();

        return this;
    }

    public IServiceCollection Build()
        => _services;
}