using Play.Items.Domain.ValueObjects;

namespace Play.Items.Application.Factories;

public interface IArtifactFactory
{
    Task<Artifact> Create(string artifactName, IDictionary<string, int> stats);
}