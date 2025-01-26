using System.Buffers;
using Play.Common.Abs.Commands;
using Play.Common.Abs.RabbitMq;
using Play.Operation.Api.Services;

namespace Play.Operation.Api.Commands.Items.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IOperationStatusService _operationStatusService;
    private readonly ICorrelationContext _correlationContext;

    public CreateItemHandler(IOperationStatusService operationStatusService,
        ICorrelationContextAccessor correlationContextAccessor)
    {
        _operationStatusService = operationStatusService;
        _correlationContext = correlationContextAccessor.CorrelationContext;
    }

    public async Task HandleAsync(CreateItem command)
    {
        await _operationStatusService.UpdateStatus(_correlationContext.UserId, _correlationContext.CorrelationId,
            "Pending");
    }
}