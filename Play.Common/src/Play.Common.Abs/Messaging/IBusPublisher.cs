namespace Play.Common.Abs.Messaging;

public interface IBusPublisher
{
    Task PublishAsync<TMessage>(TMessage message);
}