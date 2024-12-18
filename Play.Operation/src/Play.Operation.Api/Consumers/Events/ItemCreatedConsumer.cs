using MassTransit;
using Play.Items.Contracts.Events;

namespace Play.Operation.Api.Consumers.Events;

public class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    public Task Consume(ConsumeContext<ItemCreated> context)
    {
        Console.WriteLine($"ItemCreated, type: '{context.Message.GetType().Name}', correlationId: '{context.CorrelationId}'");
    
        return Task.CompletedTask;
    }
}