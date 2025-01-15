using MassTransit;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Entities;

namespace Play.Items.Tests.Shared.Fixtures.Consumers;

public class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    private readonly Func<Guid, TaskCompletionSource<Item>, Task> _onMessageReceived;
    
    public ItemCreatedConsumer(Func<Guid, TaskCompletionSource<Item>, Task> onMessageReceived)
    {
        _onMessageReceived = onMessageReceived;
    }
    
    public async Task Consume(ConsumeContext<ItemCreated> context)
    {
        var tcs = new TaskCompletionSource<Item>();
        
        await _onMessageReceived(context.Message.ItemId, tcs);
    }
}