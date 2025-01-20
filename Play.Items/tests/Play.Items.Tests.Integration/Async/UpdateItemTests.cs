using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;

namespace Play.Items.Tests.Integration.Async;

[Collection("AsyncTests")]
public class UpdateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<RabbitMqFixture>,
    IClassFixture<MongoDbFixture<Item>>
{
    //private Task Act(UpdateItem command) => _rabbitMqFixture.PublishAsync(command);

    // [Fact]
    // public async Task update_item_command_should_update_document_with_given_id_and_data()
    // {
    //     await _mongoDbFixture.InsertAsync(_item);
    //     var tcs = _rabbitMqFixture.SubscribeAndGet<ItemUpdated, Item>(_mongoDbFixture.GetAsync, _item.Id);
    //     
    //     var command = new UpdateItem(_item.Id, "Greatest book", "Even more spells", 22);
    //     await Act(command);
    //     
    //     var document = await tcs.Task;
    //     document.ShouldNotBeNull();
    //     document.Name.Value.ShouldBe("Greatest book");
    // }

    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly MongoDbFixture<Item> _mongoDbFixture;
    private readonly Item _item;
    
    public UpdateItemTests(PlayItemsApplicationFactory factory,
        RabbitMqFixture rabbitMqFixture,
        MongoDbFixture<Item> mongoDbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _rabbitMqFixture = rabbitMqFixture;
        _mongoDbFixture = mongoDbFixture;
        _item = new Item("Book", "New spells every day", 15, DateTimeOffset.UtcNow);
    }
}