using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Queries;

namespace Play.Common.Logging;

public static class Extensions
{
    public static IServiceCollection AddLoggingCommandHandlerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        
        return services;
    }
    
    public static IServiceCollection AddLoggingQueryHandlerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        
        return services;
    }
    
    
}