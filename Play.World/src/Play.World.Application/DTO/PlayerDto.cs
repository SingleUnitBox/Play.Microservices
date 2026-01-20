namespace Play.World.Application.DTO;

public class PlayerDto
{
    public Guid PlayerId { get; set; }

    public string PlayerName { get; set; }
    
    public CoordinateDto Position { get; set; }
}