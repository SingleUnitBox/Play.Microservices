using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Play.World.Domain.Entities;
using Coordinate = Play.World.Domain.ValueObjects.Coordinate;

namespace Play.World.Infrastructure.Postgres.Configuration;

public class ItemLocationConfiguration : IEntityTypeConfiguration<ItemLocation>
{
    public void Configure(EntityTypeBuilder<ItemLocation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new(value));

        // builder.OwnsOne(x => x.Position, position =>
        // {
        //     position.Property<Point>("_point")
        //         .HasColumnName("position")
        //         .HasColumnType("geometry(Point, 4326)");
        // });
        
        builder.Property(x => x.Position)
            .HasConversion(coordinate => new Point(coordinate.Longitude, coordinate.Latitude),
                point => Coordinate.Create(point.X, point.Y))
            .HasColumnName("position")
            .HasColumnType("geometry(Point, 4326)");
        
        builder.Property(x => x.ItemId);
        builder.Property(x => x.ItemName).HasMaxLength(200);
        builder.Property(x => x.IsCollected);
        builder.Property(x => x.DroppedAt);

        builder.HasIndex(x => x.Position).HasMethod("gist");
    }
}