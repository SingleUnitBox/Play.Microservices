using Play.Items.Domain.Types;

namespace Play.Items.Domain.ValueObjects;

public sealed record Artifact
{
    public HollowType CompatibleHollow { get; }

    public IReadOnlyDictionary<string, int> Stats { get; }
} 