using MassTransit;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Entities;

namespace Play.Items.Tests.Shared.Fixtures;

public class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    private readonly Func<Guid, TaskCompletionSource<Item>, Task> _onMessageReceived;
    private readonly TaskCompletionSource<Item> _tcs;
    
    public ItemCreatedConsumer(Func<Guid, TaskCompletionSource<Item>, Task> onMessageReceived,
        TaskCompletionSource<Item> tcs)
    {
        _onMessageReceived = onMessageReceived;
        _tcs = tcs;
    }
    
    public async Task Consume(ConsumeContext<ItemCreated> context)
    {
        await _onMessageReceived(context.Message.ItemId, _tcs);
    }
}