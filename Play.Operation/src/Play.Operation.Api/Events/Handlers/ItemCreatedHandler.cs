using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using Play.Operation.Api.Services;

namespace Play.Operation.Api.Events.Handlers;

public class ItemCreatedHandler : IEventHandler<ItemCreated>
{
    private readonly IOperationStatusService _statusService;
    private readonly ICorrelationContext _correlationContext;
    private readonly ILogger<ItemCreatedHandler> _logger;

    public ItemCreatedHandler(IOperationStatusService statusService,
        ICorrelationContextAccessor correlationContextAccessor,
        ILogger<ItemCreatedHandler> logger)
    {
        _statusService = statusService;
        _logger = logger;
        _correlationContext = correlationContextAccessor?.CorrelationContext;
    }

    public async Task HandleAsync(ItemCreated @event)
    {
        _logger.LogInformation($"ItemCreated: '{@event.Name}' with id '{@event.ItemId}', CorrelationId: {_correlationContext.CorrelationId}," +
                          $" UserId: {_correlationContext.UserId}");

        // Update status to "Completed"
        await _statusService.UpdateStatus(_correlationContext.UserId, @_correlationContext.CorrelationId, "Completed");
    }
}