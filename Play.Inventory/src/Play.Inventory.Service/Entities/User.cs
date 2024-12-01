using Play.Common;

namespace Play.Inventory.Service.Entities;

public class User : IEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; }
}