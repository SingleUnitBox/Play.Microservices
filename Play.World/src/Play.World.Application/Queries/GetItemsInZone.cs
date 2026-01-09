using Play.Common.Abs.Queries;
using Play.World.Application.DTO;

namespace Play.World.Application.Queries;

public record GetItemsInZone(Guid ZoneId) : IQuery<IEnumerable<ItemLocationDto>>;