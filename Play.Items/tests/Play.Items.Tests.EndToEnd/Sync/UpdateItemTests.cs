using System.Net;
using System.Text;
using Newtonsoft.Json;
using Play.Items.Contracts.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync;

[Collection("SyncTests")]
public class UpdateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<MongoDbFixture<Item>>
{
    private Task<HttpResponseMessage> Act(UpdateItem command)
        => _client.PutAsync($"items/{command.ItemId}", GetContent(command));
    
    [Fact]
    public async Task update_item_endpoint_should_return_http_status_code_no_content()
    {
        await InsertItemAsync();

        var response = await Act(_command);

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task update_item_endpoint_should_update_document_in_database_with_given_id()
    {
        await InsertItemAsync();
        
        await Act(_command);

        var document = await _mongoDbFixture.GetAsync(_command.ItemId);
        
        document.ShouldNotBeNull();
        document.Id.Value.ShouldBe(_command.ItemId);
        document.Name.Value.ShouldBe(_command.Name);
        document.Description.Value.ShouldBe(_command.Description);
    }

    private readonly HttpClient _client;
    private readonly MongoDbFixture<Item> _mongoDbFixture;
    private readonly Guid _itemId;
    private readonly UpdateItem _command;

    public UpdateItemTests(PlayItemsApplicationFactory factory,
        MongoDbFixture<Item> mongoDbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _client = factory.CreateClient();
        _mongoDbFixture = mongoDbFixture;
        _itemId = Guid.NewGuid();
        _command = new UpdateItem(_itemId, "Shield", "Good defense", 50);
    }
    
    private async Task InsertItemAsync()
        => _mongoDbFixture.InsertAsync(Item.Create(_itemId, "Potion",
            "Heals a bit of HP", 10, DateTimeOffset.UtcNow));
    
    private StringContent GetContent(object value)
        => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
}