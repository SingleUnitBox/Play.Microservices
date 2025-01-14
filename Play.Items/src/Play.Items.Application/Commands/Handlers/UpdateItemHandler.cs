﻿using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class UpdateItemHandler(
    IItemRepository itemRepository,
    IBusPublisher busPublisher) : ICommandHandler<Contracts.Commands.UpdateItem>
{
    public async Task HandleAsync(Contracts.Commands.UpdateItem command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        item.Name = command.Name;
        item.Description = command.Description;
        item.Price = command.Price;

        await itemRepository.UpdateAsync(item);
        await busPublisher.PublishAsync(new ItemUpdated(
            item.Id, item.Name, item.Price));
    }
}