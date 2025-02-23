using Microsoft.EntityFrameworkCore;
using Play.Items.Application.Commands;
using Play.Items.Application.Events;
using Play.Items.Domain.Entities;
using Play.Items.Domain.Types;
using Play.Items.Domain.ValueObjects;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Fixtures;
using Shouldly;

namespace Play.Items.Tests.Integration.Async;

[Collection("AsyncTests")]
public class DeleteItemTests : IClassFixture<PlayItemsApplicationFactory>,
    IClassFixture<ItemsPostgresDbFixture>,
    IClassFixture<RabbitMqFixture>
{
    private Task Act(DeleteItem command) => _rabbitMqFixture.PublishAsync(command);

    [Fact]
    public async Task delete_item_command_should_delete_existing_item_from_database()
    {
        var crafter = await _dbFixture.GetCrafter(_crafterId);
        _item.SetCrafter(crafter);
        _item.SetElement(Element.Create(Elements.Fire.ToString()));
        await _dbFixture.InsertItem(_item);
        
        var tcs = await _rabbitMqFixture
            .SubscribeAndGet<ItemDeleted, Item>(_dbFixture.GetNullItemAsync, _item.Id);

        await Act(new DeleteItem(_item.Id));
        
        var itemFromDb = await tcs.Task;
        itemFromDb.ShouldBeNull();
        var items = await _dbFixture.DbContext.Items.AsNoTracking().ToListAsync();
        items.Count.ShouldBe(0);
        items.ShouldBeEmpty();
    }

    private readonly ItemsPostgresDbFixture _dbFixture;
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly Item _item;
    private readonly CrafterId _crafterId;
    
    public DeleteItemTests(ItemsPostgresDbFixture dbFixture,
        RabbitMqFixture rabbitMqFixture)
    {
        _dbFixture = dbFixture;
        _rabbitMqFixture = rabbitMqFixture;
        _item = Item.Create("Book", "New spells every day", 15, DateTimeOffset.UtcNow);
        _crafterId = new CrafterId(Guid.Parse("b69f5ef7-bf93-4de2-a62f-064652d8dd19"));
    }
}