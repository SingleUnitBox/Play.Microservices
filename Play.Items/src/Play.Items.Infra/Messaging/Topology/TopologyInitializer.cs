using Microsoft.Extensions.Hosting;
using Play.Common.Abs.Commands;
using Play.Common.RabbitMq;
using Play.Common.RabbitMq.Topology;

namespace Play.Items.Infra.Messaging.Topology;

public class TopologyInitializer(ITopologyBuilder topologyBuilder) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface)
            .ToList();

        foreach (var commandType in commandTypes)
        {
            topologyBuilder.CreateTopologyAsync(
                commandType.GetExchangeName(),
                commandType.GetQueueName(),
                "",
                TopologyType.Direct, 
                stoppingToken);
        }
        
        return Task.CompletedTask;
    }
}