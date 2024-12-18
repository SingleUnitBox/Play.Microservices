using MassTransit;
using Play.Items.Contracts.Events.RejectedEvents;

namespace Play.Operation.Api.Consumers.RejectedEvents;

public class CreateItemRejectedConsumer : IConsumer<CreateItemRejected>
{
    public Task Consume(ConsumeContext<CreateItemRejected> context)
    {
        Console.WriteLine($"CreateItemRejected, type: '{context.Message.GetType().Name}', correlationId: '{context.CorrelationId}'");
    
        return Task.CompletedTask;
    }
}