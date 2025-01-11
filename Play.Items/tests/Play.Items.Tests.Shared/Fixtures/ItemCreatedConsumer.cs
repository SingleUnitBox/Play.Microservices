using MassTransit;
using Play.Items.Contracts.Events;

namespace Play.Items.Tests.Shared.Fixtures;

public class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    public Task Consume(ConsumeContext<ItemCreated> context)
    {
        return Task.CompletedTask;
    }
}