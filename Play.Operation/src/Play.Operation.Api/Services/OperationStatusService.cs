using System.Buffers;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Play.Operation.Api.DTO;
using Play.Operation.Api.Hubs;

namespace Play.Operation.Api.Services;

public class OperationStatusService : IOperationStatusService
{
    private readonly ConcurrentDictionary<Guid, OperationStatusDto> _operationStatuses = new();
    private readonly IHubContext<PlayHub> _hubContext;

    public OperationStatusService(IHubContext<PlayHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task UpdateStatus(Guid correlationId, string status, string reason = null)
    {
        if (_operationStatuses.TryGetValue(correlationId, out var operationStatus))
        {
            operationStatus.Status = status;
            operationStatus.Reason = reason;
        }
        
        await _hubContext.Clients.All.SendAsync("OperationStatusUpdated", correlationId, status, reason);
    }

    public Task<OperationStatusDto> GetStatus(Guid correlationId)
    {
        _operationStatuses.TryGetValue(correlationId, out var operationStatus);
        return Task.FromResult(operationStatus);
    }
}