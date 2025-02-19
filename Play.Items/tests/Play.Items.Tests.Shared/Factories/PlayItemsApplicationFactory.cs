using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Play.Common.RabbitMq.Consumers;
using Play.Items.Api;

namespace Play.Items.Tests.Shared.Factories;

public class PlayItemsApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Console.WriteLine("Setting test environment...");
        builder.UseEnvironment("test");
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType == typeof(CommandConsumerService))
                {
                    continue;
                }

                if (descriptor.ImplementationType == typeof(EventConsumerService))
                {
                    continue;
                }

                services.Remove(descriptor); // Remove background workers
            }
        });
    }

}
