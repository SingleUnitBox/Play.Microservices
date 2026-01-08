using Play.Common.Abs.Commands;
using Play.World.Domain.Entities;
using Play.World.Domain.Repositories;
using Play.World.Domain.ValueObjects;

namespace Play.World.Application.Commands.Handlers;

public class CreateZoneHandler(IZoneRepository zoneRepository) : ICommandHandler<CreateZone>
{
    public async Task HandleAsync(CreateZone command)
    {
        var boundary = ZoneBoundary.Create(command.Boundary
            .Select(p => 
                Coordinate.Create(p.Longitude, p.Latitude))
            .ToList());
        
        var zone = Zone.Create(command.ZoneName, boundary, new ZoneType(command.ZoneType));
        
        await zoneRepository.AddAsync(zone);
    }
}