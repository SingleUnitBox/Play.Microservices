using Play.Common.Abs.Commands;
using Play.Items.Domain.Repositories;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Application.Commands.Handlers;

public class EmbedArtifactHandler(IItemRepository itemRepository,
    ISocketRepository socketRepository) : ICommandHandler<EmbedArtifact>
{
    public async Task HandleAsync(EmbedArtifact command)
    {
        var item = await itemRepository.GetByIdAsync(command.ItemId);
        var socket = await socketRepository.GetByItemIdAsync(command.ItemId);
        
        var artifact = Artifact.Create("Ruby", HollowType.Stone, command.Stats);
        item.
    }
}