using Microsoft.EntityFrameworkCore;
using Play.Common.Settings;
using Play.Items.Domain.Entities;
using Play.Items.Infra.Postgres;
using Play.Items.Tests.Shared.Helpers;

namespace Play.Items.Tests.Shared.Fixtures;

public class ItemsPostgresDbFixture : IAsyncLifetime
{
    private readonly PostgresSettings _postgresSettings;
    public ItemsPostgresDbContext DbContext { get; private set; }

    public ItemsPostgresDbFixture()
    {
        _postgresSettings = SettingsHelper.GetSettings<PostgresSettings>("PostgresSettings");
        DbContext = new ItemsPostgresDbContext(
            new DbContextOptionsBuilder<ItemsPostgresDbContext>()
                .UseNpgsql(_postgresSettings.ConnectionString).Options);
    }
    
    public async Task<Item> GetItem(Guid itemId)
        => await DbContext.Items.FirstOrDefaultAsync(i => i.Id == itemId);
    
    public async Task GetItemAsync(Guid itemId, TaskCompletionSource<Item> tcs)
    {
        var item = await GetItem(itemId);
        if (item is null)
        {
            tcs.TrySetCanceled();
            return;
        }
        
        tcs.TrySetResult(item);
    }

    public async Task AddCrafter(Crafter crafter)
    {
        Console.WriteLine($"Adding crafter {crafter}");
        await DbContext.Crafters.AddAsync(crafter);
        await DbContext.SaveChangesAsync();
        Console.WriteLine($"Added crafter {crafter}");
    }
    
    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        DbContext.Dispose();
    }
}