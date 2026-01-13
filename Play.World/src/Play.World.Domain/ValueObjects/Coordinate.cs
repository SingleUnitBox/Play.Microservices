namespace Play.World.Domain.ValueObjects;

public class Coordinate
{
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }

    private Coordinate()
    {
        
    }
    
    private Coordinate(double longitude, double latitude)
    {
        if (longitude < -180 || longitude > 180)
            throw new InvalidDataException("Longitude must be between -180 and 180");
        
        if (latitude < -90 || latitude > 90)
            throw new InvalidDataException("Latitude must be between -90 and 90");

        Longitude = longitude;
        Latitude = latitude;
    }

    public static Coordinate Create(double longitude, double latitude)
        => new(longitude, latitude);

    public static Coordinate CreateRandom()
    {
        var random = new Random();
        var longitude = random.Next(-180, 180);
        var latitude = random.Next(-90, 90);
        
        return Create(latitude, longitude);
    }
}