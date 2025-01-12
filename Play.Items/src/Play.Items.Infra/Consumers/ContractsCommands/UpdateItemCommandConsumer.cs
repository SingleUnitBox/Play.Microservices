using MassTransit;
using Play.Common.Abs.Commands;
using Play.Items.Contracts.Commands;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class UpdateItemCommandConsumer(ICommandHandler<UpdateItem> handler) : IConsumer<UpdateItem>
{
    public async Task Consume(ConsumeContext<UpdateItem> context)
        => await handler.HandleAsync(context.Message);
}