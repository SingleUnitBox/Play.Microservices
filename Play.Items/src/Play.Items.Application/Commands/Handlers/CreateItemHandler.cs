using Play.Common.Abs.Commands;
using Play.Common.Abs.Exceptions;
using Play.Common.Abs.RabbitMq;
using Play.Items.Application.Events;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateItemHandler : ICommandHandler<CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;
    private readonly ICorrelationContext _correlationContext;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    
    public CreateItemHandler(IItemRepository itemRepository,
        IBusPublisher busPublisher,
        ICorrelationContextAccessor correlationContextAccessor,
        IExceptionToMessageMapper exceptionToMessageMapper)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _correlationContext = correlationContextAccessor.CorrelationContext;
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
            item = new Item(command.ItemId, command.Name, command.Description,
                command.Price, DateTimeOffset.UtcNow);
            await _itemRepository.CreateAsync(item);
            await _busPublisher.Publish(new ItemCreated(item.Id, item.Name, item.Price),
                _correlationContext);
        }
        catch (ItemAlreadyExistException ex)
        {
            var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
            await _busPublisher.Publish(rejectedEvent, _correlationContext);
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