using System.Net;
using Newtonsoft.Json;
using Play.Items.Application.DTO;
using Play.Items.Domain.ValueObjects;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync.Queries;

public class GetCrafterTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>
{
    private Task<HttpResponseMessage> Act() => _client.GetAsync($"/crafters/{_crafterId.Value}");

    [Fact]
    public async Task get_crafter_endpoint_should_return_http_status_code_not_found_when_crafter_does_not_exist()
    {
        var response = await _client.GetAsync($"/crafters/{Guid.NewGuid()}");
        
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task get_crafter_endpoint_should_return_http_status_code_ok_and_crafter_dto_when_crafter_exists()
    {
        var response = await Act();
        
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseBody = await response.Content.ReadAsStringAsync();
        var crafterDto = JsonConvert.DeserializeObject<CrafterDto>(responseBody);
        crafterDto.ShouldNotBeNull();
        crafterDto.CrafterId.ShouldBe(_crafterId.Value);
    }

    private readonly HttpClient _client;
    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly CrafterId _crafterId;

    public GetCrafterTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _client = factory.CreateClient();
        _dbFixture = dbFixture;
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
    }
}