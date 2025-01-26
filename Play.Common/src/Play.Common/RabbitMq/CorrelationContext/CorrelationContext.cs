using Play.Common.Abs.RabbitMq;

namespace Play.Common.RabbitMq.CorrelationContext;

public class CorrelationContext : ICorrelationContext
{
    public Guid CorrelationId { get; }
    public Guid UserId { get; }

    public CorrelationContext()
    {
    }
    
    public CorrelationContext(Guid correlationId, Guid userId)
    {
        CorrelationId = correlationId;
        UserId = userId;
    }
}