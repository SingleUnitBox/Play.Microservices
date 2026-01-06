namespace Play.World.Application.DTO;

public class ItemLocationDto
{
    public Guid ItemId { get; set; }
    public string ItemName { get; set; }
    public CoordinateDto Position { get; set; }
    public bool IsCollected { get; set; }
    public DateTimeOffset DroppedAt { get; set; }
}