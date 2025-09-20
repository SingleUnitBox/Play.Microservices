using RabbitMQ.Client;

namespace Play.Common.RabbitMq.Connection;

public sealed class ChannelFactory(ConnectionProvider connectionProvider) : IDisposable
{
    private readonly ThreadLocal<IModel> _consumerCache = new ThreadLocal<IModel>(true);
    private readonly ThreadLocal<IModel> _producerCache = new(true);

    public IModel CreateForConsumer() => Create(connectionProvider.ConsumerConnection, _consumerCache);
    public IModel CreateForProducer() => Create(connectionProvider.ProducerConnection, _producerCache);
    
    private IModel Create(IConnection connection, ThreadLocal<IModel> cache)
    {
        if (cache.Value is not null)
        {
            return cache.Value;
        }
        
        var channel = connection.CreateModel();
        cache.Value = channel;
        
        return channel;
    }

    public void Dispose()
    {
        foreach (var channel in _consumerCache.Values)
        {
            channel.Dispose();
        }

        foreach (var channel in _producerCache.Values)
        {
            channel.Dispose();
        }

        _consumerCache.Dispose();
        _producerCache.Dispose();
    }
}