using OpenTelemetry.Trace;
using Play.Common.Abs.RabbitMq;

namespace Play.Common.Metrics.Tracing;

public class TracingBusPublisherDecorator : IBusPublisher
{
    private readonly IBusPublisher _innerBusPublisher;
    private readonly Tracer _tracer;

    public TracingBusPublisherDecorator(IBusPublisher innerBusPublisher, TracerProvider tracerProvider)
    {
        _innerBusPublisher = innerBusPublisher;
        _tracer = tracerProvider.GetTracer("Play.RabbitMq");
    }
    
    public async Task Publish<TMessage>(
        TMessage message,
        string exchangeName = null,
        string messageId = null,
        ICorrelationContext correlationContext = null,
        IDictionary<string, object?> headers = null) where TMessage : class
    {
        using var span = _tracer.StartActiveSpan($"Span of '{typeof(TMessage).Name}'.");
        await  _innerBusPublisher.Publish(message, exchangeName, messageId, correlationContext, headers);
    }
}