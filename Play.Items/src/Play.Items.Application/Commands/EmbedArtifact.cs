using Play.Common.Abs.Commands;

namespace Play.Items.Application.Commands;

public record EmbedArtifact(Guid ItemId, string Artifact, IDictionary<string, int> Stats) : ICommand;