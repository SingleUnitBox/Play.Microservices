using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Play.Items.Application.Commands;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Types;
using Play.Items.Infra.Postgres;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.EndToEnd.Sync.Commands;

[Collection("SyncTests")]
public class CreateItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>
{
    private Task<HttpResponseMessage> Act(CreateItem command)
        => _httpClient.PostAsync("items", GetStringContent(command)); 
    
    [Fact]
    public async Task create_item_endpoint_should_return_http_status_code_created()
    {
        var response = await Act(_command);

        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task create_item_endpoint_should_add_document_with_given_id_to_database()
    {
        await Act(_command);
    
        var itemFromDb = await _dbContext.Items.SingleOrDefaultAsync(i => i.Id == _command.ItemId);
        itemFromDb.ShouldNotBeNull();
        itemFromDb.Id.Value.ShouldBe(_command.ItemId);
    }
    
    [Fact]
    public async Task create_item_endpoint_should_return_location_header_with_correct_item_id()
    {
        var response = await Act(_command);
    
         var locationHeader = response.Headers
             .FirstOrDefault(h => h.Key == "Location").Value.First();
    
        locationHeader.ShouldNotBeNull();
        locationHeader.ShouldBe($"http://localhost/Items/{_command.ItemId}");
    }

    private readonly PlayItemsApplicationFactory _factory;
    private readonly HttpClient _httpClient;
    private readonly ItemsPostgresDbContext _dbContext;
    private readonly Crafter _crafter;
    private readonly CreateItem _command;
    
    public CreateItemTests(PlayItemsApplicationFactory factory, ItemsPostgresDbFixture dbFixture)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "test");
        
        factory.Server.AllowSynchronousIO = true;
        _factory = factory;
        _httpClient = factory.CreateClient();
        _dbContext = dbFixture.DbContext;
        _crafter = Crafter.Create("Din Boon");
        _command = new("Sword", "Deals a lot of damage", 20.30m,
            _crafter.CrafterId, Elements.Fire.ToString());
        
        
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _dbContext.Crafters.Add(_crafter);
        _dbContext.SaveChanges();
    }
    
    private static StringContent GetStringContent(object value)
        => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
}
