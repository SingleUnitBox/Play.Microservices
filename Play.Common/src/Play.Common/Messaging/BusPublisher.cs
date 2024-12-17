using MassTransit;
using Play.Common.Abs.Messaging;

namespace Play.Common.Messaging;

public class BusPublisher : IBusPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BusPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task PublishAsync<TMessage>(TMessage message, Guid correlationId = default)
    {
        await _publishEndpoint.Publish(message, context =>
        {
            context.CorrelationId = correlationId;
        });
    }
}