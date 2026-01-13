using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common;
using Play.Common.Abs.Events;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Messaging;
using Play.Common.Queries;
using Play.Common.Serialization;
using Play.Common.Settings;
using Play.World.Application.Events.Items;
using Play.World.Application.Events.Items.Handlers;
using Play.World.Domain.Repositories;
using Play.World.Infrastructure.Exceptions;
using Play.World.Infrastructure.Postgres;
using Play.World.Infrastructure.Postgres.Repositories;

namespace Play.World.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddContext();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        services.AddControllers();
        
        // custom as o.UseNetTopologySuite() is required, can be refactored to be moved to Play.Common
        services.AddDbContext<WorldPostgresDbContext>(options => 
            options.UseNpgsql(
                configuration.GetSettings<PostgresSettings>(nameof(PostgresSettings)).ConnectionString,
                o => o.UseNetTopologySuite()));
        services.AddRepositories();
        
        services.AddEvents();
        services.AddQueries();
        services.AddCommands();
        // services.AddLoggingEventHandlerDecorator();
        
        services.AddRabbitMq(configuration, builder =>
        {
            builder
                .AddChannelFactory()
                .AddConnectionProvider()
                .AddBusPublisher()
                .AddEventConsumer()
                .AddCommandConsumer()
                .AddMessageExecutor()
                .AddResiliency()
                .AddTopologyInitializer();
        });

        services.AddSerialization();
        services.AddPlayMicroservice(
            configuration,
            config =>
            {
                config.AddExceptionHandling();
                config.AddSettings<ServiceSettings>(nameof(ServiceSettings));
                config.AddCustomExceptionToMessageMapper<WorldExceptionToMessageMapper>();
            });
        
        return services;
    }
}