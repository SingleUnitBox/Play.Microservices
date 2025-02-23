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
public class CreateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>,
    IClassFixture<RabbitMqFixture>
{
    private Task Act(CreateItem command) => _rabbitMqFixture.PublishAsync(command);

    [Fact]
    public async Task create_item_command_should_add_item_with_given_id_to_database()
    {
        var command = new CreateItem("Potion", "Heals a bit of HP", 10,
            _crafterId.Value, Elements.Fire.ToString());

         var tcs = await _rabbitMqFixture
             .SubscribeAndGet<ItemCreated, Item>(_dbFixture.GetItemAsync, command.ItemId);
         
         await Act(command);
         
         var itemFromDb = await tcs.Task;
         itemFromDb.ShouldNotBeNull();
         itemFromDb.Id.Value.ShouldBe(command.ItemId);
         //await Task.Delay(8000);
    }

    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly CrafterId _crafterId;
    
    public CreateItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture,
        RabbitMqFixture rabbitMqFixture)
    {
        //factory.Server.AllowSynchronousIO = true;
        _dbFixture = dbFixture;
        _rabbitMqFixture = rabbitMqFixture;
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
    }
}