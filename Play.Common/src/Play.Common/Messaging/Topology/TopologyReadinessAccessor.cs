using System.Collections.Concurrent;

namespace Play.Common.Messaging.Topology;

public class TopologyReadinessAccessor
{
    // private readonly ConcurrentDictionary<string, bool> _readinessMap = new();
    private readonly TaskCompletionSource _ready = new(TaskCreationOptions.RunContinuationsAsynchronously);

    // public void MarkTopologyProvisionStart(string source)
    // {
    //     _readinessMap.TryAdd(source, false);
    // }

    // public void MarkTopologyProvisionEnd(string source)
    // {
    //     _readinessMap[source] = true;
    // }

    public void MarkTopologyProvisionEnd()
        => _ready.TrySetResult();

    // public bool TopologyProvisioned
    // {
    //     get
    //     {
    //         return _readinessMap.Values.All(t => t);
    //     }
    // }
    
    public bool TopologyProvisioned
        => _ready.Task.IsCompleted;
}