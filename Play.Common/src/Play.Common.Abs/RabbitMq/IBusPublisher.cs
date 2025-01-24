namespace Play.Common.Abs.RabbitMq;

public interface IBusPublisher
{
    Task Publish<TMessage>(TMessage message,
        ICorrelationContext correlationContext = null,
        Guid? userId = null) where TMessage : class;
}