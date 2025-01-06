using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Play.Common.Abs.Queries;

namespace Play.Common.Logging;

public class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _queryHandler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;

    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> queryHandler,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    {
        _queryHandler = queryHandler;
        _logger = logger;
    }

    public async Task<TResult> QueryAsync(TQuery query)
    {
        _logger.LogInformation("Starting to handle query '{Query}'.", typeof(TQuery));
        var stopwatch = Stopwatch.StartNew();
        var result = await _queryHandler.QueryAsync(query);
        stopwatch.Stop();
        _logger.LogInformation("Stopping to handle query '{Query}'. " +
                               "Completed in {Stopwatch}ms.", typeof(TQuery), stopwatch.ElapsedMilliseconds);
        
        return result;
    }
}