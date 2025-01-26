using Microsoft.AspNetCore.SignalR;
using Play.Common.Abs.Events;
using Play.Common.Abs.RabbitMq;
using Play.Operation.Api.Hubs;
using Play.Operation.Api.Services;

namespace Play.Operation.Api.Events.RejectedEvents.Handlers;

public class CreateItemRejectedHandler : IEventHandler<CreateItemRejected>
{
    private readonly IOperationStatusService _statusService;
    private readonly ICorrelationContext _correlationContext;

    public CreateItemRejectedHandler(IOperationStatusService statusService,
        ICorrelationContextAccessor correlationContextAccessor)
    {
        _statusService = statusService;
        _correlationContext = correlationContextAccessor?.CorrelationContext;
    }
    
    public async Task HandleAsync(CreateItemRejected @event)
    {
        var userId = _correlationContext.UserId;
        Console.WriteLine($"CreateItemRejected: '{@event.Reason}', CorrelationId: {@_correlationContext.CorrelationId}, UserId: {userId}");

        await _statusService.UpdateStatus(userId, @_correlationContext.CorrelationId, "Rejected", @event.Reason);
    }
}