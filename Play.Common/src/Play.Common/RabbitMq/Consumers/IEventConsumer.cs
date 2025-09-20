using Play.Common.Abs.Events;

namespace Play.Common.RabbitMq.Consumers;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>(string? queueName = null, CancellationToken stoppingToken = default)
        where TEvent : class, IEvent;
}