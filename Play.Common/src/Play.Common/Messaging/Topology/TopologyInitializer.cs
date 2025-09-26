using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;

namespace Play.Common.Messaging.Topology;

public class TopologyInitializer(ITopologyBuilder topologyBuilder,
    TopologyReadinessAccessor topologyReadinessAccessor) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();
        
        var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();

        topologyReadinessAccessor.MarkTopologyProvisionStart(GetType().Name);
        
        foreach (var commandType in commandTypes)
        {
            topologyBuilder.CreateTopologyAsync(
                commandType.GetExchangeName(),
                commandType.GetQueueName(),
                "",
                TopologyType.Direct, 
                stoppingToken);
        }
        
        foreach (var eventType in eventTypes)
        {
            topologyBuilder.CreateTopologyAsync(
                eventType.GetExchangeName(),
                eventType.GetQueueName(),
                "",
                TopologyType.PublishSubscribe, 
                stoppingToken);
        }
        
        topologyReadinessAccessor.MarkTopologyProvisionEnd(GetType().Name);
        return Task.CompletedTask;
    }
}