using Play.Common.Abs.Messaging;

namespace Play.Common.Abs.RabbitMq;

public interface IBusPublisher
{
    Task PublishAsync<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        string routingKey = "",
        ICorrelationContext correlationContext = null,
        IDictionary<string, object> headers = default,
        CancellationToken cancellationToken = default)
        where TMessage : class;
}