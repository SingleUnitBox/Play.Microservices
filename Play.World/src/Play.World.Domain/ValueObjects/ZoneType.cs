using Play.World.Domain.Exceptions;

namespace Play.World.Domain.ValueObjects;

public class ZoneType
{
    public string Value { get; }

    private ZoneType()
    {
        
    }

    public ZoneType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyZoneTypeNameException();
        }
        
        Value = value;
    }
}