using System.Runtime.CompilerServices;

namespace Play.Items.Application.DTO;

public class ItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public SocketDto? Socket { get; set; }

    public ItemDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public ItemDto(Guid id, string name, string description, decimal price, DateTimeOffset createdDate)
        : this(id, name)
    {
        Description = description;
        Price = price;
        CreatedDate = createdDate;
    }
}