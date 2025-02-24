namespace Play.Inventory.Application.DTO;

public class UserStateDto
{
    public string State { get; set; }
    public bool IsActive => State.Equals("active", StringComparison.InvariantCultureIgnoreCase);
}