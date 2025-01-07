using MassTransit;
using Microsoft.Extensions.Logging;
using Play.Common.Logging.Mappers;
using Serilog;
using SmartFormat;

namespace Play.Common.Logging;

public class LoggingCommandConsumerDecorator<TCommand> : IConsumer<TCommand>
    where TCommand : class
{
    private readonly IConsumer<TCommand> _innerHandler;
    private readonly ILogger<LoggingCommandConsumerDecorator<TCommand>> _logger;
    private readonly IMessageToLogTemplateMapper _mapper;

    public LoggingCommandConsumerDecorator(IConsumer<TCommand> innerHandler,
        ILogger<LoggingCommandConsumerDecorator<TCommand>> logger,
        IMessageToLogTemplateMapper mapper)
    {
        _innerHandler = innerHandler;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<TCommand> context)
    {
        var command = context.Message;
        var template = _mapper.Map(context.Message);
        if (template is null)
        {
            await _innerHandler.Consume(context);
        }

        try
        {
            Log(command, template.Before);
            await _innerHandler.Consume(context);
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