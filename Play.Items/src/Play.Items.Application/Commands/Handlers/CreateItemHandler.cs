﻿using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Services;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly ICrafterRepository _crafterRepository;
    
    private readonly IBusPublisher _busPublisher;
    private readonly ICorrelationContext _correlationContext;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    
    private readonly IMessageBroker _messageBroker;
    private readonly IEventMapper _eventMapper;
    
    public CreateItemHandler(
        IItemRepository itemRepository,
        ICrafterRepository crafterRepository,
        
        IBusPublisher busPublisher,
        ICorrelationContextAccessor correlationContextAccessor,
        IExceptionToMessageMapper exceptionToMessageMapper,
        
        IMessageBroker messageBroker,
        IEventMapper eventMapper)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _correlationContext = correlationContextAccessor.CorrelationContext;
        
        _messageBroker = messageBroker;
        _eventMapper = eventMapper;
        _crafterRepository = crafterRepository;
    }

    public async Task HandleAsync(CreateItem command)
    {
        try
        {
            //throw new ItemAlreadyExistException(command.ItemId);
            var item = await _itemRepository.GetByIdAsync(command.ItemId);
             if (item != null)
             {
                 throw new ItemAlreadyExistException(item.Id);
             }
             
            // Continue processing the message
            item = Item.Create(command.ItemId, command.Name, command.Description,
                command.Price, DateTimeOffset.UtcNow);
            var crafter = await _crafterRepository.GetCrafterById(command.CrafterId);
            item.SetCrafter(crafter);
            await _itemRepository.CreateAsync(item);
            
            // await _busPublisher.Publish(new ItemCreated(item.Id, item.Name, item.Price),
            //     correlationContext: _correlationContext);
            var events = _eventMapper.MapAll(item.Events);
            await _messageBroker.PublishAsync(events);
        }
        catch (ItemAlreadyExistException ex)
        {
            var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
            await _busPublisher.Publish(rejectedEvent, correlationContext: _correlationContext);
            throw;
        }
        catch (Exception ex)
        {
            throw;
            // General exception handling
            // You could log, publish a fault message, etc.
        }
    }
}