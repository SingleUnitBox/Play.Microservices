using Play.Items.Domain.DomainEvents;
using Play.Items.Domain.Entities;
using Shouldly;
using Xunit;

namespace Play.Items.Tests.Unit.Domain.Entities;

public class ItemTests
{
    [Fact]
    public void update_name_should_update_item_name_and_add_name_updated_event()
    {
        var item = GetItem();
        var nameToBeUpdated = "Potion";
        
        item.UpdateName(nameToBeUpdated);
        
        item.Name.Value.ShouldBe(nameToBeUpdated);
        item.Events.SingleOrDefault().ShouldBeOfType<NameUpdated>();
    }

    [Fact]
    public void set_crafter_should_update_crafter()
    {
        var item = GetItem();
        var crafter = GetCrafter();
        
        item.SetCrafter(crafter);
        
        item.Crafter.CrafterId.Value.ShouldBe(crafter.CrafterId.Value);
        item.Crafter.Name.Value.ShouldBe(crafter.Name.Value);
    }

    private Item GetItem()
        => Item.Create("Sword", "Deals a lot of damage", 20.30m, DateTimeOffset.Now);
    
    private Crafter GetCrafter()
        => Crafter.Create("Din Goo");
}