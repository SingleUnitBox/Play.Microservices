using Play.Items.Application.Factories;
using Play.Items.Domain.ValueObjects;
using Play.Items.Infra.Exceptions;

namespace Play.Items.Infra.Postgres.Factories;

public class ArtifactFactory(IArtifactDefinitionRepository artifactDefinitionRepository) : IArtifactFactory
{
    public async Task<Artifact> Create(string artifactName, IDictionary<string, int> stats)
    {
        var artifactDefinition = await artifactDefinitionRepository.GetByNameAsync(artifactName);
        if (artifactDefinition is null)
        {
            throw new ArtifactDefinitionNotFoundException(artifactName);
        }

        return stats.Any() 
            ? Artifact.Create(artifactDefinition.Name, artifactDefinition.CompatibleHollowType, stats)
            : Artifact.Create(artifactDefinition.Name, artifactDefinition.CompatibleHollowType, artifactDefinition.BaseStats.ToDictionary());
    }
}