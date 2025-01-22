using Play.Common.Abs.Events;

namespace Play.Items.Infra.Consumers;

public interface IEventConsumer
{
    Task ConsumeEvent<TEvent>() where TEvent : class, IEvent;
}