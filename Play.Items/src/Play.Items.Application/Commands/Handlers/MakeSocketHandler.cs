using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Application.Commands.Handlers;

public class MakeSocketHandler(
    IItemRepository itemRepository,
    IEventProcessor eventProcessor) : ICommandHandler<MakeSocket>
{
    public async Task HandleAsync(MakeSocket command)
    {
        if (!HollowHelper.TryCreateHollowType(command.HollowType, out var hollowType))
        {
            throw new InvalidHollowTypeException(command.HollowType);
        }
        
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }
        
        var socket = Socket.Create(hollowType);
        item.MakeSocket(socket);
        
        await itemRepository.UpdateAsync(item);
        await eventProcessor.Process(item.Events);
    }
}