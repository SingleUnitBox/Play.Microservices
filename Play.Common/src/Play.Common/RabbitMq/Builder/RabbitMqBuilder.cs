using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Common.RabbitMq.Connection;
using Play.Common.RabbitMq.Consumers;

namespace Play.Common.RabbitMq.Builder;

public class RabbitMqBuilder(IServiceCollection services) : IRabbitMqBuilder
{
    public IServiceCollection Services { get; } = services;

    public IRabbitMqBuilder AddEventConsumer()
    {
        Services.AddSingleton<IEventConsumer, EventConsumer>();
        Services.AddHostedService<EventConsumerService>();

        return this;
    }

    public IRabbitMqBuilder AddCommandConsumer()
    {
        Services.AddSingleton<ICommandConsumer>(sp =>
        {
            var commandConsumer = new CommandConsumer(
                sp.GetRequiredService<ConnectionProvider>().ConsumerConnection,
                sp.GetRequiredService<ICommandDispatcher>(),
                sp,
                sp.GetRequiredService<IExceptionToMessageMapper>(),
                sp.GetRequiredService<IBusPublisher>());
            
            return commandConsumer;
        });
        Services.AddHostedService<CommandConsumerService>();

        return this;
    }

    public IServiceCollection Build()
        => Services;
}