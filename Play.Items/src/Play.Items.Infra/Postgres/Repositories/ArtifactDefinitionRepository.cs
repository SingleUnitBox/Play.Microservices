using Microsoft.EntityFrameworkCore;
using Play.Items.Application.Factories;

namespace Play.Items.Infra.Postgres.Repositories;

internal sealed class ArtifactDefinitionRepository(
    ItemsPostgresDbContext dbContext) : IArtifactDefinitionRepository
{
    public async Task<ArtifactDefinitionDto> GetByNameAsync(string artifactName)
    {
        return await dbContext.ArtifactDefinitions
            .Where(a => a.Name == artifactName)
            .Select(a => new ArtifactDefinitionDto(a.Name, a.CompatibleHollowType, a.BaseStats))
            .SingleOrDefaultAsync();
    }
}