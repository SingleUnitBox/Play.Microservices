using Play.Common.Settings;
using Play.Items.Application.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.Integration.Async;

public class CreateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<MongoDbFixture<Item>>,
    IClassFixture<RabbitMqFixture>
{
    private Task Act(CreateItem command) => _rabbitMqFixture.PublishAsync(command);

    [Fact]
    public async Task create_item_command_should_add_document_with_given_id_to_database()
    {
        var command = new CreateItem("Potion", "Heals a bit of HP", 10);

        await Task.Delay(10_000);
        //var tcs = _rabbitMqFixture;
        
        //create [keyId] = tcs
        await Act(command);

        // wait for completion
        _rabbitMqFixture.WaitForMessageAsync(command.ItemId);
        
        var document = await _mongoDbFixture.GetAsync(command.ItemId);
        document.ShouldNotBeNull();
        document.Id.Value.ShouldBe(command.ItemId);
    }

    private readonly MongoDbFixture<Item> _mongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    
    public CreateItemTests(PlayItemsApplicationFactory factory,
        MongoDbFixture<Item> mongoDbFixture,
        RabbitMqFixture rabbitMqFixture)
    {
        _mongoDbFixture = mongoDbFixture;
        _rabbitMqFixture = rabbitMqFixture;
    }
}