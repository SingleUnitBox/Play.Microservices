namespace Play.Common.Abs.RabbitMq;

public interface IBusPublisher
{
    Task Publish<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        ICorrelationContext correlationContext = null,
        IDictionary<string, object?> headers = default)
        where TMessage : class;
}