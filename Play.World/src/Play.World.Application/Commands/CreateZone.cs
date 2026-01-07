using Play.Common.Abs.Commands;
using Play.World.Application.DTO;

namespace Play.World.Application.Commands;

public record CreateZone(IEnumerable<CoordinateDto> Boundary, string ZoneName) : ICommand;