using Play.Common.Abs.SharedKernel;
using Play.World.Domain.ValueObjects;

namespace Play.World.Domain.Entities;

public class Zone : AggregateRoot
{
    public string Name { get; private set; }
    
    public ZoneBoundary Boundary { get; private set; }
    
    public ZoneType Type { get; private set; }
    
    private Zone()
    {
        Id = Guid.NewGuid();
    }

    public Zone(string name, ZoneBoundary boundary, ZoneType type) : this()
    {
        Name = name;
        Boundary = boundary;
        Type = type;
    }
    
    public static Zone Create(string name, ZoneBoundary boundary, ZoneType type)
        => new(name, boundary, type);
}