using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Messaging;
using Play.Common.Settings;
using Play.World.Domain.Repositories;
using Play.World.Infrastructure.Postgres;
using Play.World.Infrastructure.Postgres.Repositories;

namespace Play.World.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // custom as o.UseNetTopologySuite() is required, can be refactored to be moved to Play.Common
        services.AddDbContext<WorldPostgresDbContext>(options => 
            options.UseNpgsql(
                configuration.GetSettings<PostgresSettings>(nameof(PostgresSettings)).ConnectionString,
                o => o.UseNetTopologySuite()));

        services.AddScoped<IItemLocationsRepository, ItemLocationRepository>();
        services.AddRabbitMq(configuration, builder =>
        {
            builder
                .AddChannelFactory()
                .AddConnectionProvider()
                .AddBusPublisher()
                .AddEventConsumer()
                .AddTopologyInitializer();
        });
        
        return services;
    }
}