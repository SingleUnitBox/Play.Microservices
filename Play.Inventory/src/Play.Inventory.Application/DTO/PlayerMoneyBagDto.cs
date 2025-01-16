namespace Play.Inventory.Application.DTO;

public class PlayerMoneyBagDto
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; }
    public decimal Gold { get; set; }
}