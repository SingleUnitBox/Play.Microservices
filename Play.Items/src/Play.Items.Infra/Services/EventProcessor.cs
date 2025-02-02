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
        
        
    }
}