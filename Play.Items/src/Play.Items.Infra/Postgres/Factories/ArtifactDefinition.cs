using Play.Items.Domain.Types;

namespace Play.Items.Infra.Postgres.Factories;

public class ArtifactDefinition
{
    public string Name { get; set; }

    public HollowType CompatibleHollowType { get; set; }

    public IReadOnlyDictionary<string, int> BaseStats { get; set; }
}