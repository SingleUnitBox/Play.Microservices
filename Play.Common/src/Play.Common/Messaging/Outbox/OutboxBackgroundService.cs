using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Outbox.Data;
using Play.Common.Serialization;
using Play.Common.Settings;

namespace Play.Common.Messaging.Outbox;

internal class OutboxBackgroundService(IServiceProvider serviceProvider,
    OutboxDbContext dbContext,
    OutboxSettings outboxSettings,
    IMessageOutbox messageOutbox,
    IBusPublisher busPublisher,
    ISerializer serializer,
    ILogger<OutboxBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(stoppingToken);
            
            try
            {
                var unprocessedMessages = await messageOutbox.GetUnsentAsync(outboxSettings.BatchSize, stoppingToken);
                if (unprocessedMessages.Any() && unprocessedMessages.Count > 0)
                {
                    logger.LogInformation($"Found '{unprocessedMessages.Count}' unprocessed messages. Publishing...");
                    foreach (var message in unprocessedMessages)
                    {
                        await PublishOutboxMessageAsync(message, stoppingToken);
                    }
                }
                
                await transaction.CommitAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(stoppingToken);
                logger.LogError("Failed to publish outbox messages.");
            }

            await Task.Delay(outboxSettings.IntervalMilliseconds, stoppingToken);
        }
    }

    private async Task PublishOutboxMessageAsync(OutboxMessage message, CancellationToken stoppingToken)
    {
        var messageType = Type.GetType(message.MessageType);
        var deserializedMessage = serializer.Deserialize(message.SerializedMessage, messageType);

        await (Task)busPublisher.GetType()
            .GetMethod(nameof(IBusPublisher.PublishAsync))
            .MakeGenericMethod(messageType)
            .Invoke(busPublisher, new[] {deserializedMessage, message.Destination, message.MessageId, message.RoutingKey, null, message.Headers, stoppingToken});
        
        await messageOutbox.MarkAsProcessedAsync(message, stoppingToken);
        logger.LogInformation($"Outbox message '{messageType.Name}' with id '{message.MessageId}' marked as processed.");
    }
}