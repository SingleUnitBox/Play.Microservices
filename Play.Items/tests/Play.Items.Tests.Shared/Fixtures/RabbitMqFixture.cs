using System.Collections.Concurrent;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using Play.Items.Application.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Infra.Consumers.ContractsCommands;
using Play.Items.Tests.Shared.Fixtures.Consumers;
using Play.Items.Tests.Shared.Helpers;
using ZstdSharp.Unsafe;
using CreateItem = Play.Items.Contracts.Commands.CreateItem;

namespace Play.Items.Tests.Shared.Fixtures;

public class RabbitMqFixture
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBusControl _busControl;
    
    public RabbitMqFixture()
    {
        _rabbitMqSettings = SettingsHelper.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        _serviceProvider = new ServiceCollection()
            .AddMassTransit(x =>
            {
                x.AddConsumer<ItemCreatedConsumer>();
                x.AddConsumer<ItemUpdatedConsumer>();
                x.UsingRabbitMq((ctx, config) =>
                {
                    config.Host(_rabbitMqSettings.Host);
                    config.ReceiveEndpoint("item-created-queue", ep 
                        => ep.ConfigureConsumer<ItemCreatedConsumer>(ctx));
                    config.ReceiveEndpoint("item-updated-queue", ep
                        => ep.ConfigureConsumer<ItemUpdatedConsumer>(ctx));
                });
            })
            .AddSingleton(this)
            .BuildServiceProvider();
        
        _busControl = _serviceProvider.GetRequiredService<IBusControl>();
        _busControl.Start();
    }

    public async Task PublishAsync<TMessage>(TMessage message)
    {
        await _busControl.Publish(message);
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
}