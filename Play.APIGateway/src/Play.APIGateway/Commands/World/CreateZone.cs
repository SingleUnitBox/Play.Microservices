using Play.Common.Abs.Commands;

namespace Play.APIGateway.Commands.World;

public record CreateZone(IEnumerable<CoordinateDto> Boundary, string ZoneName, string ZoneType) : ICommand;