using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.RabbitMq;
using Play.Common.Messaging.Outbox.Data;
using Play.Common.Serialization;
using Play.Common.Settings;

namespace Play.Common.Messaging.Outbox;

internal class OutboxBackgroundService(IServiceProvider serviceProvider,
    IMessageOutbox messageOutbox,
    ILogger<OutboxBackgroundService> logger) : BackgroundService
{
    private OutboxDbContext _dbContext;
    private OutboxSettings _outboxSettings;
    private ISerializer _serializer;
    private IBusPublisher _busPublisher;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<OutboxDbContext>();
            _outboxSettings = scope.ServiceProvider.GetRequiredService<OutboxSettings>();
            _serializer = scope.ServiceProvider.GetRequiredService<ISerializer>();
            var publishers = scope.ServiceProvider.GetServices<IBusPublisher>();
            var outboxPublishChannel = scope.ServiceProvider.GetRequiredService<OutboxPublishChannel>();
            _busPublisher = publishers.SingleOrDefault(p => p is RabbitMqBusPublisher);
            
            ProcessOutboxChannelAsync(outboxPublishChannel, stoppingToken);
            
            while (stoppingToken.IsCancellationRequested is false)
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(stoppingToken);
            
                try
                {
                    var unprocessedMessages = new List<OutboxMessage>();
                        // await messageOutbox.GetUnsentAsync(_outboxSettings.BatchSize, stoppingToken);
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

                await Task.Delay(_outboxSettings.IntervalMilliseconds, stoppingToken);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void ProcessOutboxChannelAsync(OutboxPublishChannel outboxPublishChannel, CancellationToken stoppingToken)
    {
        await foreach (var message in outboxPublishChannel.ReadAsync(stoppingToken))
        {
            await PublishOutboxMessageAsync(message, stoppingToken);
        }
    }

    private async Task PublishOutboxMessageAsync(OutboxMessage message, CancellationToken stoppingToken)
    {
        var messageType = Type.GetType(message.MessageType);
        var deserializedMessage = _serializer.Deserialize(message.SerializedMessage, messageType);

        await (Task)_busPublisher.GetType()
            .GetMethod(nameof(IBusPublisher.PublishAsync))
            .MakeGenericMethod(messageType)
            .Invoke(_busPublisher, new[] {deserializedMessage, message.Destination, message.MessageId, message.RoutingKey, null, message.Headers, stoppingToken});
        
        await messageOutbox.MarkAsProcessedAsync(message, stoppingToken);
        logger.LogInformation($"Outbox message '{messageType.Name}' with id '{message.MessageId}' marked as processed.");
    }
}