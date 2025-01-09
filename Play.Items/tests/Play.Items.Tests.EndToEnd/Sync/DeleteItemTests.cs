using System.Net;
using System.Text;
using Newtonsoft.Json;
using Play.Items.Application.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync;

[Collection("SyncTests")]
public class DeleteItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<MongoDbFixture<Item>>
{
    private Task<HttpResponseMessage> Act()
        => _httpClient.DeleteAsync($"items/{_itemId}");

    [Fact]
    public async Task delete_item_endpoint_should_return_http_status_code_no_content()
    {
        await InsertItemAsync();

        var response = await Act();

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task delete_item_endpoint_should_remove_document_from_database_with_given_id()
    {
        await InsertItemAsync();

        await Act();
        
        var document = await _mongoDbFixture.GetAsync(_itemId);
        document.ShouldBeNull();
    }

    private readonly HttpClient _httpClient;
    private readonly MongoDbFixture<Item> _mongoDbFixture;
    private readonly Guid _itemId;
    private readonly DeleteItem _command;
    

    public DeleteItemTests(PlayItemsApplicationFactory factory,
        MongoDbFixture<Item> mongoDbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _httpClient = factory.CreateClient();
        _mongoDbFixture = mongoDbFixture;
        _itemId = Guid.NewGuid();
        _command = new DeleteItem(_itemId);
    }

    private async Task InsertItemAsync()
        => _mongoDbFixture.InsertAsync(new Item(_itemId, "Potion", "Heals a bit of HP", 10));
    
    private StringContent GetContent(object value)
        => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
}