using System.Net;
using Microsoft.EntityFrameworkCore;
using Play.Items.Domain.Entities;
using Play.Items.Infra.Postgres;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync.Commands;

[Collection("SyncTests")]
public class DeleteItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>
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
    public async Task delete_item_endpoint_should_remove_item_from_database_with_given_id()
    {
        await InsertItemAsync();

        await Act();
        
        var itemFromDb = await _dbContext.Items.SingleOrDefaultAsync(i => i.Id == _itemId);
        itemFromDb.ShouldBeNull();
    }

    private readonly HttpClient _httpClient;
    private readonly ItemsPostgresDbContext _dbContext;
    private readonly Crafter _crafter;
    private readonly Guid _itemId;
    

    public DeleteItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _httpClient = factory.CreateClient();
        _dbContext = dbFixture.DbContext;
        _crafter = Crafter.Create("Din Boon");
        _itemId = Guid.NewGuid();
    }

    private async Task InsertItemAsync()
    {
        var item = Item.Create(_itemId, "Sword", "Deals a lot of damage", 20.30m, DateTimeOffset.Now);
        item.SetCrafter(_crafter);
        var element = Element.Create("Fire");
        item.SetElement(element);
        await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();
    }
}