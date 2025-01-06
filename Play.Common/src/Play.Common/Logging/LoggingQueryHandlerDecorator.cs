using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Queries;

namespace Play.Common.Logging;

public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;
    private readonly Stopwatch _stopwatch;

    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    {
        _queryHandler = queryHandler;
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    public async Task<TResult> QueryAsync(TQuery query)
    {
        _logger.LogInformation("Starting to handle query '{QueryName}'.", typeof(TQuery));
        _stopwatch.Start();
        var result = await _queryHandler.QueryAsync(query);
        _stopwatch.Stop();
        _logger.LogInformation("Stopping to handle query '{QueryName}'. " +
                               "Completed in {StopwatchElapsed}ms.", typeof(TQuery), _stopwatch.ElapsedMilliseconds);
        
        return result;
    }
}