using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging;
using Play.Items.Application.Services;

namespace Play.Items.Infra.Services;

internal sealed class MessageBroker : IMessageBroker
{
    private readonly IBusPublisher _busPublisher;

    public MessageBroker(IBusPublisher busPublisher)
    {
        _busPublisher = busPublisher;
    }
    
    public Task PublishAsync(params IEvent[] events)
        => PublishAsync(events?.AsEnumerable());

    public async Task PublishAsync(IEnumerable<IEvent> events)
    {
        if (events is null || !events.Any())
        {
            return;
        }

        foreach (var @event in events)
        {
            if (@event is null)
            {
                continue;
            }

            var messageId = Guid.NewGuid().ToString("N");
            await _busPublisher.PublishAsync(@event, @event.GetType().GetExchangeName());
        }
    }
}