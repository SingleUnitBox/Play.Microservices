using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Repositories;

namespace Play.Items.Application.Commands.Handlers;

public class CreateCrafterHandler : ICommandHandler<CreateCrafter>
{
    private readonly ICrafterRepository _crafterRepository;

    public CreateCrafterHandler(ICrafterRepository crafterRepository)
    {
        _crafterRepository = crafterRepository;
    }

    public async Task HandleAsync(CreateCrafter command)
    {
        var crafter = await _crafterRepository.GetCrafterById(command.CrafterId);
        if (crafter is not null)
        {
            throw new CrafterAlreadyExistsException(command.CrafterId);
        }
        
        crafter = Crafter.Create(command.Name);
        await _crafterRepository.AddCrafter(crafter);
    }
}