using System.Buffers;
using Play.Operation.Api.DTO;

namespace Play.Operation.Api.Services;

public interface IOperationStatusService
{
    Task UpdateStatus(Guid userId, Guid correlationId, string status, string reason = null);
    Task<OperationStatusDto> GetStatus(Guid correlationId);
}