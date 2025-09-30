using Play.Common.Messaging.Deduplication.Data;
using Play.Common.Messaging.Executor;

namespace Play.Common.Messaging.Deduplication.FilterSteps;

internal class DeduplicationBeforeStep(PostgresDeduplicationStore deduplicationStore) : IMessageFilterStep
{
    public FilterStepType Type => FilterStepType.Before;

    public async Task ExecuteAsync(MessageProperties messageProperties, Func<Task> nextStep, CancellationToken cancellationToken = default)
    {
        var messageId = messageProperties.MessageId;
        if (await deduplicationStore.ExistsAsync(messageId, cancellationToken))
        {
            throw new MessageExecutionAbortedException($"Message '{messageProperties.MessageType}' with id {messageId} has been processed already.");
        }
        
        await nextStep();
    }
}