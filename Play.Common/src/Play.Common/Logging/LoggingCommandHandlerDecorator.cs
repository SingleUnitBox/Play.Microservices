using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;

namespace Play.Common.Logging;

public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _innerHandler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;
    private readonly Stopwatch _stopwatch;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
    {
        _innerHandler = innerHandler;
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    public async Task HandleAsync(TCommand command)
    {
        _logger.LogInformation("Starting to handle command '{CommandName}'.", typeof(TCommand));
        _stopwatch.Start();
        await _innerHandler.HandleAsync(command);
        _stopwatch.Stop();
        _logger.LogInformation("Stopping to handle command '{CommandName}'. " +
                               "Completed in {StopwatchElapsed}ms.", typeof(TCommand), _stopwatch.ElapsedMilliseconds);
    }
}