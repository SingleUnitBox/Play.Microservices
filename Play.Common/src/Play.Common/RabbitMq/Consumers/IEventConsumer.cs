using Play.Common.Abs.Events;

namespace Play.Common.RabbitMq.Consumers;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>(CancellationToken stoppingToken) where TEvent : class, IEvent;
}