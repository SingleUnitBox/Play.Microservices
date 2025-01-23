namespace Play.Common.Abs.RabbitMq;

public interface ICorrelationContextAccessor
{
    ICorrelationContext CorrelationContext { get; set; }
}