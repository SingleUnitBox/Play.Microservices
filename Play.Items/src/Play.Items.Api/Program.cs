using MassTransit;
using MongoDB.Driver;
using Play.Common.Abs.Events;
using Play.Common.Cache;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.Messaging;
using Play.Common.MongoDb;
using Play.Common.Queries;
using Play.Common.Settings;
using Play.Items.Contracts.Events;
using Play.Items.Domain.Repositories;
using Play.Items.Infra.Consumers.ContractsCommands;
using Play.Items.Infra.Exceptions;
using Play.Items.Infra.Logging;
using Play.Items.Infra.Repositories;
using Play.Items.Infra.Repositories.Cached;

namespace Play.Items.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddExceptionHandling();
        builder.Services.AddCustomExceptionToMessageMapper<ExceptionToMessageMapper>();
        //builder.Services.AddCustomExceptionToResponseMapper<CatalogExceptionMapper>();
        builder.Services.AddMassTransit(configure =>
        {
            configure.AddConsumer<CreateItemCommandConsumer>();
            configure.UsingRabbitMq((ctx, config) =>
            {
                var rabbitMqSettings = builder.Configuration.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
                config.Host(rabbitMqSettings.Host);
                config.Publish<IEvent>(p => p.Exclude = true);
                
                config.ConfigureEndpoints(ctx);
            });
        });
        builder.Services.AddMassTransitHostedService();
        builder.Services.AddContext();
        builder.Services.AddMessaging();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCommands();
        builder.Services.AddLoggingCommandHandlerDecorator();

        builder.Services.AddQueries();
        builder.Services.AddLoggingQueryHandlerDecorator();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
        
        builder.Services.AddMongoDb(builder.Configuration);
        builder.Services.AddMongoRepository<IItemRepository, ItemRepository>(
            db => new ItemRepository(db, "items"));
        builder.Services.AddScoped<ItemRepository>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            var itemRepository = new ItemRepository(db, "items");

            return itemRepository;
        });

        //caching
        builder.Services.AddScoped<IItemRepository, CachedItemRepository>();
        //builder.Services.AddMemoryCache();
        builder.Services.AddCaching();

        builder.Host.UseSerilogWithSeq();
        builder.Services.AddSingleton<IMessageToLogTemplateMapper, LocalMessageToLogTemplateMapper>();
        var app = builder.Build();

        //app.UseExceptionHandling();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", (ctx) =>
            {
                var settings = ctx.RequestServices
                    .GetService<IConfiguration>()
                    .GetSettings<ServiceSettings>(nameof(ServiceSettings));
                
                return ctx.Response.WriteAsJsonAsync($"Hello from Play.{settings.ServiceName}.Service");
            });
            endpoints.MapGet("/file", async () =>
            {
                string filePath = "C:\\Users\\czlom\\source\\repos\\Play.Microservices\\Play.Catalog\\static\\file.txt";

                var fileContents = await File.ReadAllTextAsync(filePath);
                return Results.Text(fileContents, "text/plain");
            });
        });
#pragma warning restore ASP0014

        app.Run();
    }
}
