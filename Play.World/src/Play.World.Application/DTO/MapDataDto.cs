namespace Play.World.Application.DTO;

public record MapDataDto()
{
    public IEnumerable<ItemLocationDto> ItemLocations { get; init; }
}
