using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Queries;
using Play.Common.Logging.Mappers;
using SmartFormat;

namespace Play.Common.Logging;

public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;
    private readonly IMessageToLogTemplateMapper _mapper;

    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger,
        IMessageToLogTemplateMapper mapper)
    {
        _queryHandler = queryHandler;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TResult> QueryAsync(TQuery query)
    {
        // _logger.LogInformation("Starting to handle query '{QueryName}'.", typeof(TQuery));
        // var result = await _queryHandler.QueryAsync(query);
        // _logger.LogInformation("Stopping to handle query '{QueryName}'. " +
        //                        "Completed in {StopwatchElapsed}ms.", typeof(TQuery), _stopwatch.ElapsedMilliseconds);
        var template = _mapper.Map(query);
        try
        {
            Log(template.Before, query);
            var result = await _queryHandler.QueryAsync(query);
            Log(template.After, query);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void Log(string message, TQuery query)
    {
        _logger.LogInformation(Smart.Format(message, query));
    }
}