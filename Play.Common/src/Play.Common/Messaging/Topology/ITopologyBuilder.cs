namespace Play.Common.Messaging.Topology;

public interface ITopologyBuilder
{
    Task CreateTopologyAsync(
        string publisherSource,
        string consumerDestination,
        string routingKey,
        TopologyType topologyType,
        CancellationToken cancellationToken);
}