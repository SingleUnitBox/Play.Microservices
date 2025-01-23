namespace Play.Common.Abs.RabbitMq;

public interface ICorrelationContext
{
    Guid CorrelationId { get; }
}