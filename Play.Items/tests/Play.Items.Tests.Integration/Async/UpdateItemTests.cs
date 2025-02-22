using Play.Items.Application.Commands;
using Play.Items.Application.Events;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.Integration.Async;

[Collection("AsyncTests")]
public class UpdateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>,
    IClassFixture<RabbitMqFixture>
{
    private Task Act(UpdateItem command) => _rabbitMqFixture.PublishAsync(command);

     [Fact]
     public async Task update_item_command_should_update_document_with_given_id_and_data()
     {
         var crafter = await _dbFixture.GetCrafter(_crafterId);
         _item.SetCrafter(crafter);
         _item.SetElement(Element.Create(Elements.Fire.ToString()));
         await _dbFixture.InsertItem(_item);
         
         var tcs = await _rabbitMqFixture
             .SubscribeAndGet<ItemUpdated, Item>(_dbFixture.GetItemAsync, _item.Id);
         
         var command = new UpdateItem(_item.Id, "Greatest book", "Even more spells", 22);
         await Act(command);
         
         var updatedItem = await tcs.Task;
         updatedItem.ShouldNotBeNull();
         updatedItem.Name.Value.ShouldBe("Greatest book");
         updatedItem.Price.Value.ShouldBe(22);
     }

    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly Item _item;
    private readonly CrafterId _crafterId;
    
    public UpdateItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture,
        RabbitMqFixture rabbitMqFixture)
    {
        //factory.Server.AllowSynchronousIO = true;
        _dbFixture = dbFixture;
        _rabbitMqFixture = rabbitMqFixture;
        _item = Item.Create("Book", "New spells every day", 15, DateTimeOffset.UtcNow);
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
    }
}