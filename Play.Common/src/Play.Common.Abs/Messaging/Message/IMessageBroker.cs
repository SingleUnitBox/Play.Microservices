using Play.Common.Abs.Events;

namespace Play.Common.Abs.Messaging.Message;

public interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
    Task PublishAsync(IEnumerable<IEvent> events);
}