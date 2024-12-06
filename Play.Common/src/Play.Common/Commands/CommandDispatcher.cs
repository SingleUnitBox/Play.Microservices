using Microsoft.Extensions.DependencyInjection;
using Play.Common.Temp.Commands;


namespace Play.Common.Commands;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        //var commandHandlerType = typeof(ICommandHandler<TCommand>);
        using var scope = _serviceProvider.CreateScope();
        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        
        await commandHandler.HandleAsync(command);
        // return (Task)commandHandlerType.GetMethod(nameof(ICommandHandler<TCommand>.HandleAsync))
        //     ?.Invoke(command, new[] { command });
    }
}