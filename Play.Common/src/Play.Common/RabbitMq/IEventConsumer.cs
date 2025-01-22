using Play.Common.Abs.Events;

namespace Play.Common.RabbitMq;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>() where TEvent : class, IEvent;
}