using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IItemRepository _itemRepository;
    
    public CreateItemHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task HandleAsync(CreateItem command)
    {
        try
        {
            var item = await _itemRepository.GetByIdAsync(command.ItemId);
            if (item != null)
            {
                throw new ItemAlreadyExistException(item.Id);
            }

            // Continue processing the message
            item = new Item(command.ItemId, command.Name, command.Description,
                command.Price, DateTimeOffset.UtcNow);
            await _itemRepository.CreateAsync(item);
            // await _busPublisher.PublishAsync(new ItemCreated(item.Id, item.Name, item.Price),
            //     //context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
            //     Guid.Empty);
        }
        catch (ItemAlreadyExistException ex)
        {
            throw;
            //var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
            // await _busPublisher.PublishAsync(rejectedEvent, 
            //     context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
        }
        catch (Exception ex)
        {
            throw;
            // General exception handling
            // You could log, publish a fault message, etc.
        }
    }
}