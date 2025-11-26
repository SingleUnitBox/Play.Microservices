using Microsoft.Extensions.Logging;
using Play.Common.Messaging.Executor;
using Play.Common.Messaging.Outbox.Data;
using Play.Common.Settings;

namespace Play.Common.Messaging.Outbox.FilterSteps;

internal class OutboxAfterStep(OutboxLocalCache cache,
    OutboxPublishChannel outboxPublishChannel,
    OutboxSettings settings,
    ILogger<OutboxAfterStep> logger) : IMessageFilterStep
{
    public FilterStepType Type => FilterStepType.After;
    
    public async Task ExecuteAsync(MessageProperties messageProperties, Func<Task> nextStep, CancellationToken cancellationToken = default)
    {
        if (settings.PublishOnCommit)
        {
            var outboxMessages = cache.GetForPublish();
            logger.LogInformation($"Publishing on commit enabled. Publishing '{outboxMessages.Count}' outbox messages...");

            foreach (var message in outboxMessages)
            {
                await outboxPublishChannel.PublishAsync(message, cancellationToken);
            }
        }
    }
}