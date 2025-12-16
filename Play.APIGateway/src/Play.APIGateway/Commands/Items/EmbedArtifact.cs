using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.Items;

public record EmbedArtifact(Guid ItemId, string ArtifactName, IDictionary<string, int> Stats) : ICommand;