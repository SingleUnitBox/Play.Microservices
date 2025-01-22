using Play.Common.Abs.Events;

namespace Play.Common.RabbitMq.Consumers;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>() where TEvent : class, IEvent;
}