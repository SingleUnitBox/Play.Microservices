using System.Net;
using Newtonsoft.Json;
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
    private Task<HttpResponseMessage> Act() => _client.GetAsync("/items");
    
    [Fact]
    public async Task get_items_endpoint_should_return_http_status_code_ok()
    {
        var response = await Act();
        
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task get_items_endpoint_should_return_collection_of_item_dtos()
    {
        var crafter = await _dbFixture.GetCrafter(_crafterId);
        var bookItem = Item.Create("Book", "New spells every day", 5, DateTimeOffset.UtcNow);
        bookItem.SetCrafter(crafter);
        bookItem.SetElement(Element.Create(Elements.Fire.ToString()));
        var stoneItem = Item.Create("Stone", "Magic in the rock", 10, DateTimeOffset.UtcNow);
        stoneItem.SetCrafter(crafter);
        stoneItem.SetElement(Element.Create(Elements.Water.ToString()));
        var scrollItem = Item.Create("Scroll", "Ancient item", 15, DateTimeOffset.UtcNow);
        scrollItem.SetCrafter(crafter);
        scrollItem.SetElement(Element.Create(Elements.Earth.ToString()));
        var itemsToBeInserted = new List<Item>()
        {
            bookItem,
            stoneItem,
            scrollItem
        };
        await _dbFixture.InsertItems(itemsToBeInserted);
        
        var response = await _client.GetAsync("/items");
        
        var responseBody = await response.Content.ReadAsStringAsync();
        var itemDtos = JsonConvert.DeserializeObject<List<ItemDto>>(responseBody);
        
        itemDtos.ShouldNotBeEmpty();
        itemDtos.Count.ShouldBe(3);
    }

    private readonly HttpClient _client;
    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly CrafterId _crafterId;
    
    public GetItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _client = factory.CreateClient();
        _dbFixture = dbFixture;
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
    }
}