

using Play.Common.Abs.Commands;

namespace Play.Common.Messaging.Executor;

[MessageExecutorDecorator]
public class MessageExecutorCommandHandlerDecorator<TCommand>
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly IMessageExecutor _messageExecutor;
    private readonly ICommandHandler<TCommand> _innerHandler;

    public MessageExecutorCommandHandlerDecorator(IMessageExecutor messageExecutor,
        ICommandHandler<TCommand> innerHandler)
    {
        _messageExecutor = messageExecutor;
        _innerHandler = innerHandler;
    }
    public async Task HandleAsync(TCommand command)
    {
        await _messageExecutor.ExecuteAsync(async () => await _innerHandler.HandleAsync(command), CancellationToken.None);
    }
}