using Play.Common.Abs.Events;

namespace Play.Items.Application.Services;

public interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
    Task PublishAsync(IEnumerable<IEvent> events);
}