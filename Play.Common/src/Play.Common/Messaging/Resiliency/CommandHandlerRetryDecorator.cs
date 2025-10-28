using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Settings;
using Polly;

namespace Play.Common.Messaging.Resiliency;

public class CommandHandlerRetryDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _innerHandler;
    private ILogger<CommandHandlerRetryDecorator<TCommand>> _logger;
    private readonly RetryStrategySettings _retrySettings;
    
    public CommandHandlerRetryDecorator(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandHandlerRetryDecorator<TCommand>> logger,
        ResiliencySettings resiliencySettings)
    {
        _innerHandler = innerHandler;
        _logger = logger;
        _retrySettings = new() 
        {
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = resiliencySettings.Consumer.ConsumerRetriesLimit,
            OnRetry = onRetryArgs =>
            {
                if (onRetryArgs.AttemptNumber < resiliencySettings.Consumer.ConsumerRetriesLimit - 1)
                {
                    _logger.LogWarning($"Consume failed - attempt to retry via consumer - attempt '{onRetryArgs.AttemptNumber+1}'");
                }
                else
                {
                    logger.LogError("Consumer retries limit reached.");
                }
                
                return ValueTask.CompletedTask;
            }
        };
    }
    
    public async Task HandleAsync(TCommand command)
    {
        var retryPolicy = new ResiliencePipelineBuilder()
            .AddRetry(_retrySettings)
            .Build();
        
        await retryPolicy.ExecuteAsync(async _ => await _innerHandler.HandleAsync(command));
    }
}