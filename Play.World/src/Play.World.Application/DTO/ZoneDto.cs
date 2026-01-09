using Play.World.Domain.ValueObjects;

namespace Play.World.Application.DTO;

public class ZoneDto
{
    public Guid ZoneId { get; set; }
    
    public string Name { get; set; }

    public ZoneBoundaryDto Boundary { get; set; }

    public string Type { get; set; }
}