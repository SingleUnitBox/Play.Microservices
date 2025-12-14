using Play.Items.Domain.Types;

namespace Play.Items.Domain.ValueObjects;

public sealed record Artifact
{
    public string Name { get; }
    
    public HollowType CompatibleHollow { get; }

    public IReadOnlyDictionary<string, int> Stats { get; }

    private Artifact(string name, HollowType compatibleHollow, IReadOnlyDictionary<string, int> stats)
    {
        Name = name;
        CompatibleHollow = compatibleHollow;
        Stats = stats;
    }
    
    public static Artifact Create(
        string name,
        HollowType compatibleHollow,
        IDictionary<string, int> stats)
        => new(
            name,
            compatibleHollow,
            new Dictionary<string, int>(stats));
} 