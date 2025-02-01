using System.Collections.ObjectModel;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Crafter
{
    public CrafterId CrafterId { get; }
    public CrafterName Name { get; }
    public IReadOnlyCollection<Item> Items { get; } = new List<Item>();

    private Crafter()
    {
        
    }
    
    private Crafter(string crafterName)
    {
        CrafterId = Guid.NewGuid();
        Name = crafterName;
    }

    public static Crafter Create(string crafterName)
        => new Crafter(crafterName);
}