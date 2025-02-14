using Play.Common.Abs.Commands;
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
    private readonly IElementRepository _elementRepository;
    
    private readonly IBusPublisher _busPublisher;
    private readonly ICorrelationContext _correlationContext;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
    
    private readonly IEventProcessor _eventProcessor;
    
    public CreateItemHandler(
        IItemRepository itemRepository,
        ICrafterRepository crafterRepository,
        
        IBusPublisher busPublisher,
        ICorrelationContextAccessor correlationContextAccessor,
        IExceptionToMessageMapper exceptionToMessageMapper,
        
        IEventProcessor eventProcessor,
        IElementRepository elementRepository)
    {
        _itemRepository = itemRepository;
        _crafterRepository = crafterRepository;
        
        _busPublisher = busPublisher;
        _exceptionToMessageMapper = exceptionToMessageMapper;
        _correlationContext = correlationContextAccessor.CorrelationContext;
        
        _eventProcessor = eventProcessor;
        _elementRepository = elementRepository;
    }

    public async Task HandleAsync(CreateItem command)
    {
        var item = await _itemRepository.GetByIdAsync(command.ItemId);
         if (item != null)
         {
             throw new ItemAlreadyExistException(item.Id);
         }
         
        item = Item.Create(
            command.ItemId,
            command.Name,
            command.Description,
            command.Price,
            DateTimeOffset.UtcNow);
        var crafter = await _crafterRepository.GetCrafterById(command.CrafterId);
        item.SetCrafter(crafter);
        var element = await _elementRepository.GetElement(command.Element);
        item.SetElement(element);
        await _itemRepository.CreateAsync(item);
        await _eventProcessor.Process(item.Events);
    }
}