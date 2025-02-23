using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Play.Items.Application.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Infra.Postgres;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync.Commands;

[Collection("SyncTests")]
public class UpdateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>
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
    public async Task update_item_endpoint_should_update_item_in_database_with_given_id()
    {
        await InsertItemAsync();
        
        await Act(_command);

        var itemFromDb = await _dbContext.Items
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == _command.ItemId);

        itemFromDb.ShouldNotBeNull();
        itemFromDb.Id.Value.ShouldBe(_command.ItemId);
        itemFromDb.Name.Value.ShouldBe(_command.Name);
        itemFromDb.Description.Value.ShouldBe(_command.Description);
    }

    private readonly HttpClient _client;
    private readonly ItemsPostgresDbContext _dbContext;
    private readonly Crafter _crafter;
    private readonly Guid _itemId;
    private readonly UpdateItem _command;

    public UpdateItemTests(PlayItemsApplicationFactory factory,
        ItemsPostgresDbFixture dbFixture)
    {
        factory.Server.AllowSynchronousIO = true;
        _client = factory.CreateClient();
        _dbContext = dbFixture.DbContext;
        _crafter = Crafter.Create("Din Boon");
        _itemId = Guid.NewGuid();
        _command = new UpdateItem(_itemId, "Shield", "Good defense", 50);
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
    
    private StringContent GetContent(object value)
        => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
}