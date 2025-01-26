using System.Buffers;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using Play.Operation.Api.DTO;
using Play.Operation.Api.Hubs;
using Play.Operation.Api.Infrastructure;

namespace Play.Operation.Api.Services;

public class OperationStatusService : IOperationStatusService
{
    private readonly ConcurrentDictionary<Guid, OperationStatusDto> _operationStatuses = new();
    private readonly IHubContext<PlayHub> _hubContext;

    public OperationStatusService(IHubContext<PlayHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task UpdateStatus(Guid userId, Guid correlationId, string status, string reason = null)
    {
        var operationStatus = new OperationStatusDto
        {
            CorrelationId = correlationId,
            Status = status,
            Reason = reason
        };
        _operationStatuses[correlationId] = operationStatus;

        var group = userId.ToString("N").ToUserGroup();
        Console.WriteLine($"Sending status update to group {group}: {status}");
        //await _hubContext.Clients.Group(group).SendAsync("OperationStatusUpdated", correlationId, status, reason);
        switch (status)
        {
            case "Pending": 
                await _hubContext.Clients.Group(group)
                .SendAsync("operation_pending", correlationId, status, reason);
                break;
            case "Completed":
                await _hubContext.Clients.Group(group)
                    .SendAsync("operation_completed", correlationId, status, reason);
                break;
            case "Rejected":
                await _hubContext.Clients.Group(group)
                    .SendAsync("operation_rejected", correlationId, status, reason);
                break;
        }
    }

    public Task<OperationStatusDto> GetStatus(Guid correlationId)
    {
        _operationStatuses.TryGetValue(correlationId, out var operationStatus);
        return Task.FromResult(operationStatus);
    }
}