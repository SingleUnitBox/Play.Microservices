using Microsoft.Extensions.Hosting;
using Play.Common.Messaging.Consumers;
using Play.Items.Application.Commands;

namespace Play.Items.Infra.Services.Consumers;

public class CreateItemConsumerService(ICommandConsumer commandConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await commandConsumer.ConsumeCommand<CreateItem>(stoppingToken);
    }
}