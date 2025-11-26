using Play.Common.Messaging.Executor;
using Play.Common.Messaging.Outbox.Data;

namespace Play.Common.Messaging.Outbox.FilterSteps;

internal class OutboxBeforeStep(OutboxLocalCache cache) : IMessageFilterStep
{
    public FilterStepType Type => FilterStepType.Before;
    
    public async Task ExecuteAsync(MessageProperties messageProperties, Func<Task> nextStep, CancellationToken cancellationToken = default)
    {
        cache.Initialize();
        await nextStep();
    }
}