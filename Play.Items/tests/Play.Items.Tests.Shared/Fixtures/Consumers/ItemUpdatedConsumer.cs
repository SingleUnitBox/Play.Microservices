using MassTransit;
using Play.Items.Domain.Entities;

namespace Play.Items.Tests.Shared.Fixtures.Consumers;

public class ItemUpdatedConsumer //: IConsumer<ItemUpdated>
{
    // private readonly Func<Guid, TaskCompletionSource<Item>, Task> _onMessageReceived;
    // private readonly TaskCompletionSource<Item> _tcs;
    //
    // public ItemUpdatedConsumer(Func<Guid, TaskCompletionSource<Item>, Task> onMessageReceived,
    //     TaskCompletionSource<Item> tcs)
    // {
    //     _onMessageReceived = onMessageReceived;
    //     _tcs = tcs;
    // }
    //
    // public async Task Consume(ConsumeContext<ItemUpdated> context)
    // {
    //     await _onMessageReceived(context.Message.ItemId, _tcs);
    // }
}