using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.RabbitMq;
using Play.Common.Settings;
using Play.Items.Tests.Shared.Factories;
using Play.Items.Tests.Shared.Helpers;

namespace Play.Items.Tests.Shared.Fixtures;

public class RabbitMqFixture : IAsyncLifetime
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private IBusPublisher _busPublisher;
    
    public RabbitMqFixture()
    {
        _rabbitMqSettings = SettingsHelper.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
    }

    public async Task PublishAsync<TMessage>(TMessage message) where TMessage : class
    {
        await _busPublisher.Publish(message);
    }
    
    public TaskCompletionSource<TEntity> SubscribeAndGet<TMessage, TEntity>(
        Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
    {
        //create tcs
        var tcs = new TaskCompletionSource<TEntity>();
        //when ItemCreated arrives, got to GetAsync() to try GetItemById and set it as tcs.Result
        // is that really needed?
        onMessageReceived(id, tcs);
        //return tcs with Item Task<Item> inside
        return tcs;
    }

    public async Task InitializeAsync()
    {
        var factory = new PlayItemsApplicationFactory();
        var scope = factory.Services.CreateScope();
        _busPublisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}