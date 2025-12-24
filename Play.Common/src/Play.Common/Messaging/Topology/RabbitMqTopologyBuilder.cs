using Microsoft.Extensions.Logging;
using Play.Common.Messaging.Connection;
using Play.Common.Settings;
using RabbitMQ.Client;

namespace Play.Common.Messaging.Topology;

public class RabbitMqTopologyBuilder(
    ChannelFactory channelFactory, 
    TopologySettings topologySettings,
    ILogger<RabbitMqTopologyBuilder> logger) : ITopologyBuilder
{
    public Task CreateTopologyAsync(
        string publisherSource,
        string consumerDestination,
        string routkingKey,
        TopologyType topologyType,
        CancellationToken cancellationToken)
    {
        if (!topologySettings.Enabled)
        {
            return Task.CompletedTask;
        }

        var channel = channelFactory.CreateForConsumer();

        switch (topologyType)
        {
            case TopologyType.Direct:
                CreateDirect(publisherSource, consumerDestination, routkingKey, channel);
                break;
            case TopologyType.PublishSubscribe:
                CreateTopic(publisherSource, consumerDestination, routkingKey, channel);
                break;
            default:
                throw new NotImplementedException($"{nameof(topologyType)} is not supported");
        }

        return Task.CompletedTask;
    }

    private void CreateDirect(string publisherSource, string consumerDestination, string routkingKey, IModel channel)
    {
        if (!string.IsNullOrWhiteSpace(publisherSource))
        {
            logger.LogInformation($"Declaring exchange name of '{publisherSource}'.");
            channel.ExchangeDeclare(publisherSource, ExchangeType.Direct, durable: true);
        }
        
        logger.LogInformation($"Declaring queue name of '{consumerDestination}'.");
        channel.QueueDeclare(consumerDestination, durable: true, exclusive: false, autoDelete: false);

        if (!string.IsNullOrWhiteSpace(publisherSource))
        {
            channel.QueueBind(queue: consumerDestination, exchange: publisherSource, routingKey: routkingKey);
        }
    }
    
    private void CreateTopic(string publisherSource, string consumerDestination, string routkingKey, IModel channel)
    {
        logger.LogInformation($"Declaring exchange name of '{publisherSource}'.");
        channel.ExchangeDeclare(publisherSource, ExchangeType.Topic, durable: true);
        
        logger.LogInformation($"Declaring queue name of '{consumerDestination}'.");
        channel.QueueDeclare(consumerDestination, durable: true, exclusive: false, autoDelete: false);
        
        channel.QueueBind(queue: consumerDestination, exchange: publisherSource,
            routingKey: string.IsNullOrEmpty(routkingKey)
                ? "#"
                : routkingKey);
    }
}