using System.Text;
using Newtonsoft.Json;
using Play.Items.Domain.Entities;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;

namespace Play.Items.Tests.EndToEnd.Sync;

[Collection("SyncTests")]
public class CreateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<MongoDbFixture<Item>>
{
    // private Task<HttpResponseMessage> Act(CreateItem command)
    //     => _httpClient.PostAsync("items", GetContent(command));

    [Fact]
    public async Task create_item_endpoint_should_return_http_status_code_created()
    {
        //var response = await Act(_command);

        //response.ShouldNotBeNull();
        //response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task create_item_endpoint_should_add_document_with_given_id_to_database()
    {
        //await Act(_command);

        //var document = await _mongoDbFixture.GetAsync(_command.ItemId);
        
         //document.ShouldNotBeNull();
         //document.Id.Value.ShouldBe(_command.ItemId);
    }

    [Fact]
    public async Task create_item_endpoint_should_return_location_header_with_correct_item_id()
    {
        //var response = await Act(_command);

        // var locationHeader = response.Headers
        //     .FirstOrDefault(h => h.Key == "Location").Value.First();

        //locationHeader.ShouldNotBeNull();
        //locationHeader.ShouldBe($"http://localhost/Items/{_command.ItemId}");
    }

    private readonly HttpClient _httpClient;
    private readonly MongoDbFixture<Item> _mongoDbFixture;
   // private readonly CreateItem _command = new("Potion", "Heals a bit of HP", 10);

    public CreateItemTests(PlayItemsApplicationFactory factory, MongoDbFixture<Item> mongoDbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _httpClient = factory.CreateClient();
        _mongoDbFixture = mongoDbFixture;
    }
    
    private static StringContent GetContent(object value)
        => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
}