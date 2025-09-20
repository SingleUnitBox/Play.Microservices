namespace Play.Common.RabbitMq.Topology;

public interface ITopologyBuilder
{
    Task CreateTopologyAsync(
        string publisherSource,
        string consumerDestination,
        string filter,
        TopologyType topologyType,
        CancellationToken cancellationToken);
}