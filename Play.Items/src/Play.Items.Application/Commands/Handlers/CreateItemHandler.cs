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

public class CreateItemHandler(IItemRepository itemRepository,
    ICrafterRepository crafterRepository,
    IElementRepository elementRepository,
    IEventProcessor eventProcessor) : ICommandHandler<CreateItem>
{

    public async Task HandleAsync(CreateItem command)
    {
         var item = await itemRepository.GetByIdAsync(command.ItemId);
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
         var crafter = await crafterRepository.GetCrafterById(command.CrafterId);
         if (crafter is null)
         {
             throw new CrafterNotFoundException(command.CrafterId);
         }

         item.SetCrafter(crafter);
         var element = await elementRepository.GetElement(command.Element);
         item.SetElement(element);
         await itemRepository.CreateAsync(item);
         await eventProcessor.Process(item.Events);
    }
}