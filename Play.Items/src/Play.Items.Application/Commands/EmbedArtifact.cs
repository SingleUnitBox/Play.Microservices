using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record EmbedArtifact(Guid ItemId, string ArtifactName, IDictionary<string, int> Stats) : ICommand;