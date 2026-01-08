using Play.Common.Abs.Queries;
using Play.World.Application.DTO;

namespace Play.World.Application.Queries;

public record GetZones : IQuery<IEnumerable<ZoneDto>>;