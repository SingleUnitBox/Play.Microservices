using MongoDB.Driver;
using Play.Common.Cache;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.MassTransit;
using Play.Common.Messaging;
using Play.Common.MongoDb;
using Play.Common.Queries;
using Play.Items.Domain.Repositories;
using Play.Items.Infra.Exceptions;
using Play.Items.Infra.Repositories;
using Play.Items.Infra.Repositories.Cached;
using Serilog;

namespace Play.Items.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddExceptionHandling();
        builder.Services.AddCustomExceptionToMessageMapper<ExceptionToMessageMapper>();
        //builder.Services.AddCustomExceptionToResponseMapper<CatalogExceptionMapper>();
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
        // builder.Services.AddMassTransit(configure =>
        // {
        //     configure.SetKebabCaseEndpointNameFormatter();
        //     configure.UsingRabbitMq((ctx, cfg) =>
        //     {
        //         var rabbitMqSettings = builder.Configuration
        //             .GetSection(nameof(RabbitMqSettings))
        //             .Get<RabbitMqSettings>();
        //         cfg.Host(rabbitMqSettings.Host);
        //     
        //         // cfg.Message<ItemCreated>(e =>
        //         // {
        //         //     e.SetEntityName("Items.ItemCreated");
        //         // });
        //         cfg.ConfigureEndpoints(ctx);
        //     });
        // });

        builder.Services.AddContext();
        builder.Services.AddMessaging();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCommands();
        builder.Services.AddLoggingCommandHandlerDecorator();
        builder.Services.AddQueries();
        builder.Services.AddLoggingQueryHandlerDecorator();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        
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
        //builder.Services.AddScoped<IItemRepository, CachedItemRepository>();
        //builder.Services.AddMemoryCache();
        //builder.Services.AddCaching();

        builder.Host.UseSerilogWithSeq();
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
            endpoints.MapGet("/", () => "Hello from Play.Catalog.Service");
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