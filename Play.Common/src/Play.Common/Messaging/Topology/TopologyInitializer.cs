using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;

namespace Play.Common.Messaging.Topology;

public class TopologyInitializer(ITopologyBuilder topologyBuilder,
    TopologyReadinessAccessor topologyReadinessAccessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();
        
        var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();

        // topologyReadinessAccessor.MarkTopologyProvisionStart(GetType().Name);

        var tasks = new List<Task>();
        tasks.AddRange(commandTypes.Select(c => topologyBuilder.CreateTopologyAsync(
            c.GetExchangeName(),
            c.GetQueueName(),
            "",
            TopologyType.Direct,
            stoppingToken)));
        
        tasks.AddRange(eventTypes.Select(e => topologyBuilder.CreateTopologyAsync(
            e.GetExchangeName(),
            e.GetQueueName(),
            "",
            TopologyType.PublishSubscribe, 
            stoppingToken)));
        
        await Task.WhenAll(tasks);
        topologyReadinessAccessor.MarkTopologyProvisionEnd();
    }
}