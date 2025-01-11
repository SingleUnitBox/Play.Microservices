using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;
using CreateItem = Play.Items.Contracts.Commands.CreateItem;

namespace Play.Items.Tests.Integration.Async;

public class CreateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<MongoDbFixture<Item>>,
    IClassFixture<RabbitMqFixture>
{
    private Task Act(CreateItem command) => _rabbitMqFixture.PublishAsync(command);

    [Fact]
    public async Task create_item_command_should_add_document_with_given_id_to_database()
    {
        //var command = new CreateItem("Potion", "Heals a bit of HP", 10);
        var contractCommand = new CreateItem("Potion", "Heals a bit of HP", 10);
        
        await Act(contractCommand);
        
        //wait for completion
        await _rabbitMqFixture.WaitForMessageAsync(contractCommand.ItemId);

        var document = await _mongoDbFixture.GetAsync(contractCommand.ItemId);
        document.ShouldNotBeNull();
        document.Id.Value.ShouldBe(contractCommand.ItemId);
    }

    private readonly MongoDbFixture<Item> _mongoDbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    
    public CreateItemTests(PlayItemsApplicationFactory factory,
        MongoDbFixture<Item> mongoDbFixture,
        RabbitMqFixture rabbitMqFixture)
    {
        _mongoDbFixture = mongoDbFixture;
        _rabbitMqFixture = rabbitMqFixture;
        factory.Server.AllowSynchronousIO = true;
    }
}