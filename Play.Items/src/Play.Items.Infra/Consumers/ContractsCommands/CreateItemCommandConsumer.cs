using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Messaging;
using Play.Common.Exceptions.Mappers;
using Play.Items.Application.Exceptions;
using Play.Items.Contracts.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Infra.Consumers.ContractsCommands;

// public class CreateItemCommandConsumer : IConsumer<CreateItem>
// {
//     private readonly IItemRepository _itemRepository;
//     private readonly IBusPublisher _busPublisher;
//     private readonly IExceptionToMessageMapper _exceptionToMessageMapper;
//
//     public CreateItemCommandConsumer(IItemRepository itemRepository,
//         IBusPublisher busPublisher,
//         IExceptionToMessageMapper exceptionToMessageMapper)
//     {
//         _itemRepository = itemRepository;
//         _busPublisher = busPublisher;
//         _exceptionToMessageMapper = exceptionToMessageMapper;
//     }
//     
//     public async Task Consume(ConsumeContext<CreateItem> context)
//     {
//         var command = context.Message;
//         try
//         {
//             //throw new ItemAlreadyExistException(command.Id);
//             var item = await _itemRepository.GetByIdAsync(context.Message.Id);
//             if (item != null)
//             {
//                 throw new ItemAlreadyExistException(item.Id);
//             }
//
//             // Continue processing the message
//             item = new Item(command.Id, command.Name, command.Description, command.Price);
//             await _itemRepository.CreateAsync(item);
//             await _busPublisher.PublishAsync(new ItemCreated(item.Id, item.Name, item.Price),
//                 context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
//         }
//         catch (ItemAlreadyExistException ex)
//         {
//             var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
//             await _busPublisher.PublishAsync(rejectedEvent, 
//                 context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
//         }
//         catch (Exception ex)
//         {
//             // General exception handling
//             // You could log, publish a fault message, etc.
//         }
//     }
// }

public class CreateItemCommandConsumer : ICommandHandler<Application.Commands.CreateItem>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBusPublisher _busPublisher;
    private readonly IExceptionToMessageMapper _exceptionToMessageMapper;

    public CreateItemCommandConsumer(IItemRepository itemRepository,
        IBusPublisher busPublisher,
        IExceptionToMessageMapper exceptionToMessageMapper)
    {
        _itemRepository = itemRepository;
        _busPublisher = busPublisher;
        _exceptionToMessageMapper = exceptionToMessageMapper;
    }
    
    public async Task HandleAsync(Application.Commands.CreateItem command)
    {
        try
        {
            //throw new ItemAlreadyExistException(command.Id);
            var item = await _itemRepository.GetByIdAsync(command.Id);
            if (item != null)
            {
                throw new ItemAlreadyExistException(item.Id);
            }

            // Continue processing the message
            item = new Item(command.Id, command.Name, command.Description, command.Price);
            await _itemRepository.CreateAsync(item);
            // await _busPublisher.PublishAsync(new ItemCreated(item.Id, item.Name, item.Price),
            //     context.CorrelationId.HasValue ? context.CorrelationId.Value : Guid.Empty);
        }
        catch (ItemAlreadyExistException ex)
        {
            var rejectedEvent = _exceptionToMessageMapper.Map(ex, command);
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

