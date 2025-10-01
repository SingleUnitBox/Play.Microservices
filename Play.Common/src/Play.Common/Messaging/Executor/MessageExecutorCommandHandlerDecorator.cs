using Play.Common.Abs.Commands;

namespace Play.Common.Messaging.Executor;

[MessageExecutorDecorator]
public class MessageExecutorCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> innerHandler,
    IMessageExecutor messageExecutor)
    : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command)
    {
        await messageExecutor.ExecuteAsync(async () => await innerHandler.HandleAsync(command), CancellationToken.None);
    }
}