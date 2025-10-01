using Play.Common.Messaging.Deduplication.Data;
using Play.Common.Messaging.Executor;

namespace Play.Common.Messaging.Deduplication.FilterSteps;

internal class DeduplicationWithinStep(IDeduplicationStore deduplicationStore) : IMessageFilterStep
{
    public FilterStepType Type => FilterStepType.Within;
    
    public async Task ExecuteAsync(MessageProperties messageProperties, Func<Task> nextStep, CancellationToken cancellationToken = default)
    {
        var messageId = messageProperties.MessageId;
        await deduplicationStore.AddEntryAsync(messageId, cancellationToken);
        
        await nextStep();
    }
}