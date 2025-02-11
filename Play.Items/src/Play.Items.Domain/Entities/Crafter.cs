using System.Collections.ObjectModel;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Crafter
{
    public CrafterId CrafterId { get; }
    public CrafterName Name { get; }
    public IReadOnlyCollection<Skill> Skills { get; } = new List<Skill>();
    public IReadOnlyCollection<Item> Items { get; } = new List<Item>();

    private Crafter()
    {
        
    }

    private Crafter(Guid crafterId, string crafterName)
    {
        CrafterId = crafterId;
        Name = crafterName;
    }
    
    private Crafter(string crafterName)
    {
        CrafterId = Guid.NewGuid();
        Name = crafterName;
    }

    public static Crafter Create(string crafterName)
        => new Crafter(crafterName);
    
    public static Crafter Create(Guid crafterId, string crafterName)
        => new Crafter(crafterId, crafterName);
}