namespace Play.Inventory.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    
    public static User Create(Guid id, string username)
        => new User { Id = id, Username = username };
}