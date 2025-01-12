using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

public class DeleteItemCommandConsumer(ICommandHandler<DeleteItem> handler) : IConsumer<DeleteItem>
{
    public async Task Consume(ConsumeContext<DeleteItem> context)
    {
        await handler.HandleAsync(context.Message);
    }
}