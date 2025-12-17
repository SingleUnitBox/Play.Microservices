using Play.Common.Abs.Commands;
using Play.Items.Application.Exceptions;
using Play.Items.Application.Factories;
using Play.Items.Application.Services;
using Play.Items.Domain.Repositories;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Application.Commands.Handlers;

public class EmbedArtifactHandler(
    IItemRepository itemRepository,
    IArtifactFactory artifactFactory,
    IEventProcessor eventProcessor) : ICommandHandler<EmbedArtifact>
{
    public async Task HandleAsync(EmbedArtifact command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        if (item is null)
        {
            throw new ItemNotFoundException(command.ItemId);
        }

        var artifact = await artifactFactory.Create(command.ArtifactName, command.Stats);
        item.EmbedArtifact(artifact);
        
        await itemRepository.UpdateAsync(item);
        await eventProcessor.Process(item.Events);
    }
}