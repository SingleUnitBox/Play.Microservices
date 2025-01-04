using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;

namespace Play.Common.Logging;

public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _innerHandler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger)
    {
        _innerHandler = innerHandler;
        _logger = logger;
    }

    public async Task HandleAsync(TCommand command)
    {
        _logger.LogInformation("Starting to handle command '{typeof(TCommand)}'.");
        var stopwatch = Stopwatch.StartNew();
        await _innerHandler.HandleAsync(command);
        stopwatch.Stop();
        _logger.LogInformation($"Stopping to handle command '{typeof(TCommand)}'" +
                               $" in {stopwatch.ElapsedMilliseconds}ms.");
    }
}