namespace Play.Common.Temp.Commands;

public interface ICommandHandler<TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command);
}