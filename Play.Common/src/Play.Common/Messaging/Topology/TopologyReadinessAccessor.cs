using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Play.Common.Messaging.Topology;

public class TopologyReadinessAccessor(ILogger<TopologyReadinessAccessor> logger)
{
    private readonly TaskCompletionSource _ready = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _initializerStarted;

    public void MarkTopologyProvisionStart()
    {
        _initializerStarted = true;
    }
    
    public void MarkTopologyProvisionEnd()
        => _ready.TrySetResult();
    
    private bool TopologyProvisioned
        => _ready.Task.IsCompleted;
    
    public async Task EnsureTopologyReadiness(CancellationToken stoppingToken)
    {
        if (_initializerStarted is false)
        {
            return;
        }

        while (TopologyProvisioned is false)
        {
            logger.LogInformation("Waiting for topology to be provisioned...");
            await Task.Delay(1_000, stoppingToken);
        }
    }
}