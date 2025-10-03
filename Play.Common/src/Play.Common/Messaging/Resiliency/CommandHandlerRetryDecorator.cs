using Play.Common.Abs.Commands;
using Play.Common.Settings;
using Polly;

namespace Play.Common.Messaging.Resiliency;

public class CommandHandlerRetryDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    public ICommandHandler<TCommand> _innerHandler { get; set; }
    public RetryStrategySettings _retrySettings { get; set; }
    
    public CommandHandlerRetryDecorator()
    {
        _retrySettings = new()
        {
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 
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