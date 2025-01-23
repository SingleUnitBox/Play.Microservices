using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Logging.Attributes;
using Play.Common.Logging.Mappers;
using SmartFormat;

namespace Play.Common.Logging;

[LoggingDecorator]
public class LoggingCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _innerHandler;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand>> _logger;
    private readonly IMessageToLogTemplateMapper _mapper;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler,
        ILogger<LoggingCommandHandlerDecorator<TCommand>> logger,
        IMessageToLogTemplateMapper mapper)
    {
        _innerHandler = innerHandler;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task HandleAsync(TCommand command)
    {
        var template = _mapper.Map(command);
        if (template is null)
        {
            await _innerHandler.HandleAsync(command);
            return;
        }

        try
        {
            Log(command, template.Before);
            await _innerHandler.HandleAsync(command);
            Log(command, template.After);
        }
        catch (Exception exception)
        {
            var exceptionTemplate = template.GetExceptionTemplate(exception);
            Log(command, exceptionTemplate, true);
            throw;
        }
    }

    private void Log(TCommand command, string message, bool isError = false)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        if (isError)
        {
            _logger.LogError(Smart.Format(message, command));
        }
        else
        {
            _logger.LogInformation(Smart.Format(message, command));
        }
    }
}