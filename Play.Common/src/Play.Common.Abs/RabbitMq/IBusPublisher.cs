namespace Play.Common.Abs.RabbitMq;

public interface IBusPublisher
{
    Task Publish<TMessage>(TMessage message, ICorrelationContext correlationContext = null)
        where TMessage : class;
}