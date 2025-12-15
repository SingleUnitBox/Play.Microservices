namespace Play.Items.Application.Factories;

public interface IArtifactDefinitionRepository
{
    Task<ArtifactDefinitionDto> GetByNameAsync(string artifactName);
}