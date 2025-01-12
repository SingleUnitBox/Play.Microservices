using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class DeleteItemsCommandConsumer(ICommandHandler<DeleteItems> handler) : IConsumer<DeleteItems>
{
    
    public async Task Consume(ConsumeContext<DeleteItems> context)
    {
        await handler.HandleAsync(context.Message);
    }
}