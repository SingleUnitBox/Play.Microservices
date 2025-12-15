using Play.Items.Domain.Types;

namespace Play.Items.Application.Factories;

public record ArtifactDefinitionDto(string Name, HollowType CompatibleHollowType, IReadOnlyDictionary<string, int> BaseStats);