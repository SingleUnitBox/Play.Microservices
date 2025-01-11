using MassTransit;
using Play.Common.Abs.Commands;
using Play.Items.Contracts.Commands;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class CreateItemCommandConsumer : IConsumer<CreateItem>
{
    private readonly ICommandHandler<CreateItem> _handler;

    public CreateItemCommandConsumer(ICommandHandler<CreateItem> handler)
    {
        _handler = handler;
    }

    public async Task Consume(ConsumeContext<CreateItem> context)
    {
        await _handler.HandleAsync(context.Message);
    }
}