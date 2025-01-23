using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;

namespace Play.Operation.Api.Events.Handlers;

public class ItemCreatedHandler : IEventHandler<ItemCreated>
{
    private readonly ICorrelationContext _correlationContext;

    public ItemCreatedHandler(ICorrelationContextAccessor correlationContextAccessor)
    {
        _correlationContext = correlationContextAccessor.CorrelationContext;
    }
    
    public Task HandleAsync(ItemCreated @event)
    {
        Console.WriteLine($"ItemCreated, type: '{@event.GetType().Name}', correlationId: '{_correlationContext.CorrelationId}'");
    
        return Task.CompletedTask;
    }
}