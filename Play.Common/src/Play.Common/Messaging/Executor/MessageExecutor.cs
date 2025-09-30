using System.Transactions;
using Microsoft.Extensions.Logging;

namespace Play.Common.Messaging.Executor;

public sealed class MessageExecutor(
    IEnumerable<IMessageFilterStep> filterSteps,
    MessagePropertiesAccessor messagePropertiesAccessor,
    ILogger<MessageExecutor> logger) : IMessageExecutor
{
    public async Task ExecuteAsync(Func<Task> handle, CancellationToken cancellationToken)
    {
        var messageProperties = messagePropertiesAccessor.InitializeIfEmpty();

        try
        {
            logger.LogInformation($"Executing filter step before transaction for message:" +
                                  $" '{messageProperties.MessageType}' with id: '{messageProperties.MessageId}'.");
            await ExecuteStepAsync(FilterStepType.Before, cancellationToken);
            
            using (var scope = BeginTransaction())
            {
                logger.LogInformation($"Executing filter step within transaction for message:" +
                                      $" '{messageProperties.MessageType}' with id: '{messageProperties.MessageId}'.");
                await handle();
                await ExecuteStepAsync(FilterStepType.Within, cancellationToken);
                
                scope.Complete();
            }

            logger.LogInformation($"Executing filter step after transaction for message:" +
                                  $" '{messageProperties.MessageType}' with id: '{messageProperties.MessageId}'.");
            await ExecuteStepAsync(FilterStepType.After, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task ExecuteStepAsync(FilterStepType filterStepType, CancellationToken cancellationToken)
    {
        var messageProperties = messagePropertiesAccessor.Get();
        var steps = filterSteps.Where(f => f.Type == filterStepType);

        var pipeline = () => Task.CompletedTask;

        foreach (var step in steps.Reverse())
        {
            var nextStep = pipeline;
            pipeline = () => step.ExecuteAsync(messageProperties!, nextStep, cancellationToken);
        }
        
        await pipeline();
    }

    private TransactionScope BeginTransaction()
    {
        var options = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(30)
        };
        
        return new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled);
    }
}