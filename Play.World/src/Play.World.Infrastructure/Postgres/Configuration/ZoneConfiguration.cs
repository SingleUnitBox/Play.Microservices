using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Play.World.Domain.Entities;
using Play.World.Domain.Exceptions;
using Play.World.Domain.ValueObjects;
using Coordinate = Play.World.Domain.ValueObjects.Coordinate;

namespace Play.World.Infrastructure.Postgres.Configuration;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new(value));
        
        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.OwnsOne(x => x.Type, type =>
        {
            type.Property(t => t.Value)
                .HasColumnName("Type")
                .HasMaxLength(100)
                .IsRequired();
        });
        
        builder.Property(x => x.Boundary)
            .HasConversion(
                boundary => CreatePolygon(boundary.Points, 4326),
                polygon => ZoneBoundary.Create(ExtractCoordinates(polygon))
            )
            .HasColumnName("Boundary")
            .HasColumnType("geometry(Polygon, 4326)");

        builder.HasIndex(x => x.Boundary).HasMethod("gist");
    }
    
    private static Polygon CreatePolygon(IReadOnlyList<Coordinate> points, int srid)
    {
        if (points == null) throw new ArgumentNullException(nameof(points));
        if (points.Count < 3) throw new InvalidZoneBoundaryException();

        // Convert domain Coordinates to NTS Coordinates (X=lon, Y=lat)
        var coords = points
            .Select(p => new NetTopologySuite.Geometries.Coordinate(p.Longitude, p.Latitude))
            .ToList();

        // Ensure ring is closed (first point = last point)
        if (!coords[0].Equals2D(coords[^1]))
        {
            coords.Add(coords[0]);
        }

        // Must have at least 4 coordinates (triangle + closing point)
        if (coords.Count < 4)
            throw new InvalidZoneBoundaryException();

        var factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid);
        var ring = factory.CreateLinearRing(coords.ToArray());

        if (!ring.IsValid)
            throw new InvalidZoneBoundaryException();

        var polygon = factory.CreatePolygon(ring);

        if (!polygon.IsValid)
            throw new InvalidZoneBoundaryException();

        return polygon; // SRID already set by factory
    }

    private static List<Coordinate> ExtractCoordinates(Polygon polygon)
    {
        if (polygon == null || polygon.IsEmpty)
            return new List<Coordinate>();

        // Get exterior ring coordinates (excluding the closing duplicate point)
        var coordinates = polygon.ExteriorRing.Coordinates;
        
        return coordinates
            .Take(coordinates.Length - 1) // Remove closing point
            .Select(c => Coordinate.Create(c.X, c.Y)) // X=Longitude, Y=Latitude
            .ToList();
    }
}