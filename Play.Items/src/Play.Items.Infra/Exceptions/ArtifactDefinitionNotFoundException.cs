using Play.Common.Abs.Exceptions;

namespace Play.Items.Infra.Exceptions;

public class ArtifactDefinitionNotFoundException : PlayException
{
    public string ArtifactName { get; }
    public ArtifactDefinitionNotFoundException(string artifactName)
        : base($"Artifact definition for '{artifactName}' not found.")
    {
        ArtifactName = artifactName;
    }
}