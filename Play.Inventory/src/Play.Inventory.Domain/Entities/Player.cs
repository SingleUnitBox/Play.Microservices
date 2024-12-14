namespace Play.Inventory.Domain.Entities;

public class Player
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    
    public static Player Create(Guid id, string username)
        => new Player { Id = id, Username = username };
}