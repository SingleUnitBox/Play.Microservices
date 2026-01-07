using Play.World.Domain.Exceptions;

namespace Play.World.Domain.ValueObjects;

public class ZoneBoundary
{
    public List<Coordinate> Points { get; private set; }
    
    public static ZoneBoundary Create(List<Coordinate> points)
    {
        if (points.Count < 3)
            throw new InvalidZoneBoundaryException();
            
        return new ZoneBoundary { Points = points };
    }
}