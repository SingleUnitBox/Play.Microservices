using Microsoft.Extensions.Logging;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;
using Play.Common.Logging.Attributes;
using Play.Common.Logging.Mappers;
using SmartFormat;

namespace Play.Common.Logging;

[LoggingDecorator]
public class LoggingEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
    where TEvent : class, IEvent
{
    private readonly IEventHandler<TEvent> _innerHandler;
    private readonly ILogger<LoggingEventHandlerDecorator<TEvent>> _logger;
    private readonly IMessageToLogTemplateMapper _mapper;

    public LoggingEventHandlerDecorator(IEventHandler<TEvent> innerHandler,
        ILogger<LoggingEventHandlerDecorator<TEvent>> logger,
        IMessageToLogTemplateMapper mapper)
    {
        _innerHandler = innerHandler;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task HandleAsync(TEvent @event)
    {
        var template = _mapper.Map(@event);
        if (template is null)
        {
            await _innerHandler.HandleAsync(@event);
            return;
        }

        try
        {
            Log(@event, template.Before);
            await _innerHandler.HandleAsync(@event);
            Log(@event, template.After);
        }
        catch (Exception exception)
        {
            var exceptionTemplate = template.GetExceptionTemplate(exception);
            Log(@event, exceptionTemplate, true);
            throw;
        }
    }

    private void Log(TEvent command, string message, bool isError = false)
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