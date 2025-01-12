using System.Collections.Concurrent;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using Play.Items.Application.Commands;
using Play.Items.Contracts.Events;
using Play.Items.Infra.Consumers.ContractsCommands;
using Play.Items.Tests.Shared.Helpers;
using ZstdSharp.Unsafe;
using CreateItem = Play.Items.Contracts.Commands.CreateItem;

namespace Play.Items.Tests.Shared.Fixtures;

public class RabbitMqFixture
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBus _bus;
    private readonly ConcurrentDictionary<Guid, TaskCompletionSource<bool>> _completionSources= new();
    

    public RabbitMqFixture()
    {
        _rabbitMqSettings = SettingsHelper.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        _serviceProvider = new ServiceCollection()
            .AddMassTransit(x =>
            {
                x.AddConsumer<ItemCreatedConsumer>();
                x.UsingRabbitMq((ctx, config) =>
                {
                    config.Host(_rabbitMqSettings.Host);
                    config.ConfigureEndpoints(ctx,
                        new KebabCaseEndpointNameFormatter(false));
                });
            })
            .AddSingleton(this)
            .BuildServiceProvider();
        _bus = _serviceProvider.GetRequiredService<IBus>();
    }

    public async Task PublishAsync<TMessage>(TMessage message)
    {
        _completionSources.TryAdd((message as CreateItem).ItemId, new TaskCompletionSource<bool>());
        await _bus.Publish(message);
    }

    public TaskCompletionSource<TEntity> SubscribeAndGet<TMessage, TEntity>(
        Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
    {
        var tcs = new TaskCompletionSource<TEntity>();

        var bucControl = Bus.Factory.CreateUsingRabbitMq(config =>
        {
            config.Host(_rabbitMqSettings.Host);
            config.ReceiveEndpoint("myTestQueue", ep =>
            {
                ep.Handler<ItemCreated>(async context =>
                {
                    await onMessageReceived(id, tcs);
                });
            });
        });
        
        bucControl.Start();

        return tcs;
    }

    public Task WaitForMessageAsync(Guid itemId)
    {
        var tcs = new TaskCompletionSource<bool>();
        _completionSources[itemId] = tcs;
        
        return Task.WhenAny(tcs.Task);
    }

    public void MessageProcessed(Guid itemId)
    {
        if (_completionSources.TryRemove(itemId, out var tcs))
        {
            tcs.SetResult(true);
        }
    }
}