namespace Play.Common.Messaging.Topology;

public interface ITopologyBuilder
{
    Task CreateTopologyAsync(
        string publisherSource,
        string consumerDestination,
        string filter,
        TopologyType topologyType,
        CancellationToken cancellationToken);
}