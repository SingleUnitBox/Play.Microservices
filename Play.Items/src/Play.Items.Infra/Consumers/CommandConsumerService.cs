using Microsoft.Extensions.Hosting;
using Play.Items.Application.Commands;

namespace Play.Items.Infra.Consumers;

public class CommandConsumerService : BackgroundService
{
    private readonly CommandConsumer _commandConsumer;
    

    public CommandConsumerService(CommandConsumer commandConsumer)
    {
        _commandConsumer = commandConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _commandConsumer.ConsumeCommand<CreateItem>();
    }
}