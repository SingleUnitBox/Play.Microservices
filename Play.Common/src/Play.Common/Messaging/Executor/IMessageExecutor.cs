namespace Play.Common.Messaging.Executor;

public interface IMessageExecutor
{
    Task ExecuteAsync(Func<Task> handle, CancellationToken cancellationToken);
}