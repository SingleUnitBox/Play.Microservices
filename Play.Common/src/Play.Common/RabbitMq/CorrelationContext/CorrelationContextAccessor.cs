using Play.Common.Abs.RabbitMq;

namespace Play.Common.RabbitMq.CorrelationContext;

public class CorrelationContextAccessor : ICorrelationContextAccessor
{
    private readonly AsyncLocal<ICorrelationContext> _correlationContext = new();

    public ICorrelationContext CorrelationContext
    {
        get => _correlationContext.Value;
        set => _correlationContext.Value = value;
    }
}