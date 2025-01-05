using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abs.Commands;

namespace Play.Common.Logging;

public static class Extensions
{
    public static IServiceCollection AddLoggingCommandHandlerDecorator(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        
        return services;
    }
}