using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Events;
using Play.Common.Abs.SharedKernel.DomainEvents;
using Play.Items.Application.Services;

namespace Play.Items.Infra.Services;

internal sealed class EventProcessor : IEventProcessor
{
    private readonly IMessageBroker _messageBroker;
    private readonly IEventMapper _eventMapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EventProcessor> _logger;

    public EventProcessor(
        IMessageBroker messageBroker,
        IEventMapper eventMapper,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<EventProcessor> logger)
    {
        _messageBroker = messageBroker;
        _eventMapper = eventMapper;
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task Process(IEnumerable<IDomainEvent> domainEvents)
    {
        if (domainEvents is null)
        {
            return;
        }

        var integrationEvents = await HandleDomainEventsAsync(domainEvents);
        if (!integrationEvents.Any())
        {
            return;
        }
        
        await _messageBroker.PublishAsync(integrationEvents.Distinct());
    }

    private async Task<List<IEvent>> HandleDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        var integrationEvents = new List<IEvent>();
        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent == null)
            {
                continue;
            }

            // var domainEventType = domainEvent.GetType();
            // var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEventType);
            // var domainEventHandlers = scope.ServiceProvider.GetServices(domainEventHandlerType);
            // foreach (var handler in domainEventHandlers)
            // {
            //     await (Task)domainEventHandlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))
            //         ?.Invoke(handler, new object[] { domainEvent })!;
            // }
            
            var domainEventType = domainEvent.GetType();
            var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEventType);
            dynamic domainEventHandlers = scope.ServiceProvider.GetServices(domainEventHandlerType);
            foreach (var handler in domainEventHandlers)
            {
                await handler.HandleAsync((dynamic) domainEvent);
            }
            
            var integrationEvent = _eventMapper.Map(domainEvent);
            if (integrationEvent is null)
            {
                continue;
            }
            
            integrationEvents.Add(integrationEvent);
        }
        
        return integrationEvents;
    }
}