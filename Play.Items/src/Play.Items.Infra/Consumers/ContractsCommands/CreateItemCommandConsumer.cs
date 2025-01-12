using MassTransit;
using Play.Common.Abs.Commands;
using Play.Items.Contracts.Commands;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class CreateItemCommandConsumer(ICommandHandler<CreateItem> handler) : IConsumer<CreateItem>
{
    public async Task Consume(ConsumeContext<CreateItem> context)
    {
        await handler.HandleAsync(context.Message);
    }
}