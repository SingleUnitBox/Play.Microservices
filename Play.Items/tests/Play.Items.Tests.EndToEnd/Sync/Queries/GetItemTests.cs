using System.Net;
using Newtonsoft.Json;
using Play.Common.Abs.SharedKernel.Types;
using Play.Items.Application.DTO;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync.Queries;

[Collection("SyncTests")]
public class GetItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>
{
    private Task<HttpResponseMessage> Act() => _client.GetAsync($"/items/{_itemId}");

    [Fact]
    public async Task get_item_endpoint_should_return_http_status_code_not_found_when_item_does_not_exist()
    {
        var response = await Act();

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task get_item_endpoint_should_return_http_status_code_ok_and_item_dto_when_item_exists()
    {
        var item = Item.Create(_itemId, "Book", "New spells every day", 5, DateTimeOffset.UtcNow);
        var crafter = await _dbFixture.GetCrafter(_crafterId);
        item.SetCrafter(crafter);
        item.SetElement(Element.Create(Elements.Fire.ToString()));
        await _dbFixture.InsertItem(item);
        
        var response = await Act();
        
        response.ShouldNotBeNull();
        var responseBody = await response.Content.ReadAsStringAsync();
        var itemDto = JsonConvert.DeserializeObject<ItemDto>(responseBody);
        itemDto.ShouldNotBeNull();
        itemDto.Id.ShouldBe(_itemId);
    }
    
    private readonly HttpClient _client;
    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly CrafterId _crafterId;
    private readonly Guid _itemId;
    
    public GetItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _client = factory.CreateClient();
        _dbFixture = dbFixture;
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
        _itemId = Guid.Parse("4167c08a-7b8a-41d5-97c3-c8a593c3f58a");
    }
}