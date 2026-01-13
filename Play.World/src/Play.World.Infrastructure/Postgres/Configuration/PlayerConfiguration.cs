using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Play.World.Domain.Entities;
using Coordinate = Play.World.Domain.ValueObjects.Coordinate;

namespace Play.World.Infrastructure.Postgres.Configuration;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new(value));
        
        
        builder.Property(x => x.Position)
            .HasConversion(
                coordinate => new Point(coordinate.Longitude, coordinate.Latitude) { SRID = 4326 },
                point => Coordinate.Create(point.X, point.Y))
            .HasColumnName("Position")
            .HasColumnType("geometry(Point, 4326)");
        builder.HasIndex(x => x.Position).HasMethod("gist");
    }
}