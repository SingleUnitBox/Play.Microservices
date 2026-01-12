using Play.Common.Abs.Queries;

namespace Play.World.Application.Queries;

public record CalculateDistance(Guid FromItemId, Guid ToItemId) : IQuery<double>;