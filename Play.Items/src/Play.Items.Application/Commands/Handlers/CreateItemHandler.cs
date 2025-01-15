using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<Contracts.Commands.CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;
    

    public CreateItemHandler(IItemRepository itemRepository,
        IBusPublisher busPublisher)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
    }

    public async Task HandleAsync(Contracts.Commands.CreateItem command)
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
            await _busPublisher.PublishAsync(new ItemCreated(item.Id, item.Name, item.Price),
                //context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
                Guid.Empty);
        }
        catch (ItemAlreadyExistException ex)
        {
            //var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
            // await _busPublisher.PublishAsync(rejectedEvent, 
            //     context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
        }
        catch (Exception ex)
        {
            // General exception handling
            // You could log, publish a fault message, etc.
        }
    }
}