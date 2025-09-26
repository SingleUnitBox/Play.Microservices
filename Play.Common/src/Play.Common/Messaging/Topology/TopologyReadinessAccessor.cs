namespace Play.Common.Messaging.Topology;

public class TopologyReadinessAccessor
{
    private readonly IDictionary<string, bool> _readinessMap = new Dictionary<string, bool>();

    public void MarkTopologyProvisionStart(string source)
    {
        _readinessMap.Add(source, false);
    }

    public void MarkTopologyProvisionEnd(string source)
    {
        _readinessMap[source] = true;
    }

    public bool TopologyProvisioned
    {
        get
        {
            return _readinessMap.Values.All(t => t);
        }
    }
}