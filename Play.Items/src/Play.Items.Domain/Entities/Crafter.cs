using System.Collections.ObjectModel;
using Play.Items.Domain.ValueObjects;

namespace Play.Items.Domain.Entities;

public class Crafter
{
    public CrafterId CrafterId { get; }
    public CrafterName Name { get; }
    public IReadOnlyCollection<Item> Items { get; } = new List<Item>();

    public Crafter(Guid crafterId, string crafterName)
    {
        CrafterId = crafterId;
        Name = crafterName;
    }
}