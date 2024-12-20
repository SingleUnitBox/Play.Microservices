﻿using GreenPipes;
using MassTransit;
using Play.Common.Abs.Messaging;
using Play.Common.Context;
using Play.Common.Exceptions.Mappers;
using Play.Items.Application.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers;

public class CreateItemConsumer : IConsumer<CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;

    public CreateItemConsumer(IItemRepository itemRepository,
        IBusPublisher busPublisher)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
    }
    
    public async Task Consume(ConsumeContext<CreateItem> context)
    {
        var scopedContext = context.GetPayload<IScopedContext>();
        scopedContext.CurrentMessage = context.Message;
        
        var command = context.Message;
        var item = await _itemRepository.GetByIdAsync(command.Id);
        if (item is not null)
        {
            throw new ItemAlreadyExistException(item.Id);
        }
        
        item = new Item(command.Id, command.Name, command.Description, command.Price);
        await _itemRepository.CreateAsync(item);
        await _busPublisher.PublishAsync(new ItemCreated(
            item.Id, item.Name, item.Price));
    }
}