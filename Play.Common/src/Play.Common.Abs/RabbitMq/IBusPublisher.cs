namespace Play.Common.Abs.RabbitMq;

public interface IBusPublisher
{
    Task Publish<TMessage>(TMessage message, string messageId = null, ICorrelationContext correlationContext = null)
        where TMessage : class;
}