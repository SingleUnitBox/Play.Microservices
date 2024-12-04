using Microsoft.Extensions.DependencyInjection;
using Play.Common.Abstractions.Commands;

namespace Play.Common.Commands;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        var commandHandlerType = typeof(ICommandHandler<TCommand>);
        var commandHandler = _serviceProvider.GetRequiredService(commandHandlerType);
        
        return (Task)commandHandlerType.GetMethod(nameof(ICommandHandler<TCommand>.HandleAsync))
            ?.Invoke(command, new[] { command });
    }
}