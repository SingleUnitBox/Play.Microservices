﻿using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class UpdateItemHandler(IItemRepository itemRepository,
    IBusPublisher busPublisher) : ICommandHandler<UpdateItem>
{
    public async Task HandleAsync(UpdateItem command)
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
        await busPublisher.PublishAsync(new Contracts.Contracts.CatalogItemUpdated(
            item.Id, item.Name, item.Price));
    }
}