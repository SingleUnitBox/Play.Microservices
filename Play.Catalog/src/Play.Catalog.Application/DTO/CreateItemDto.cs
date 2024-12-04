namespace Play.Catalog.Application.DTO;

public class CreateItemDto
{
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }

    public CreateItemDto(string name, decimal price, string description)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}