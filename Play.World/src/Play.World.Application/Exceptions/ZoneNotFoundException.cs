using Play.Common.Abs.Exceptions;

namespace Play.World.Application.Exceptions;

public class ZoneNotFoundException : PlayException
{
    public string ZoneName { get; }
    
    public ZoneNotFoundException(string zoneName)
        : base($"Zone with name '{zoneName}' was not found.")
    {
        ZoneName = zoneName;
    }
}