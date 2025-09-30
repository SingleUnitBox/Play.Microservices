namespace Play.Common.Messaging.Executor;

public interface IMessageFilterStep
{
    FilterStepType Type { get; }
    
    Task ExecuteAsync(MessageProperties messageProperties, Func<Task> nextStep,
        CancellationToken cancellationToken = default);
}

public enum FilterStepType
{
    Before,
    Within,
    After
}