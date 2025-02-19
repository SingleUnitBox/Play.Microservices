using Asp.Versioning;
using MongoDB.Driver;
using Play.Common.AppInitializer;
using Play.Common.Auth;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.MongoDb;
using Play.Common.PostgresDb;
using Play.Common.Queries;
using Play.Common.RabbitMq;
using Play.Common.Settings;
using Play.Items.Domain.Repositories;
using Play.Items.Infra;
using Play.Items.Infra.Exceptions;
using Play.Items.Infra.Logging;
using Play.Items.Infra.Postgres;
using Play.Items.Infra.Postgres.Repositories;
using Play.Items.Infra.Repositories;

namespace Play.Items.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularUI",
                builder => builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        
        builder.Services.AddExceptionHandling();
        builder.Services.AddHostedService<AppInitializer>();
        builder.Services.AddCustomExceptionToMessageMapper<ExceptionToMessageMapper>();
        builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
        builder.Services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        
        builder.Services.AddContext();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCommands();
        builder.Services.AddLoggingCommandHandlerDecorator();
        builder.Services.AddQueries();
        builder.Services.AddLoggingQueryHandlerDecorator();
        builder.Services.AddEvents();
        builder.Services.AddLoggingEventHandlerDecorator();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
        builder.Services.AddRabbitMq()
            .AddCommandConsumer()
            .AddEventConsumer()
            .Build();
        builder.Services.AddInfra();

        builder.Services.AddPostgresDb<ItemsPostgresDbContext>();
        builder.Services.AddPostgresRepositories();
        // builder.Services.AddMongoDb(builder.Configuration);
        // builder.Services.AddMongoRepository<IItemRepository, ItemRepository>(
        //     db => new ItemRepository(db, "items"));
        // builder.Services.AddScoped<ItemRepository>(sp =>
        // {
        //     var db = sp.GetRequiredService<IMongoDatabase>();
        //     var itemRepository = new ItemRepository(db, "items");
        //
        //     return itemRepository;
        // });

        //caching
        //builder.Services.AddScoped<IItemRepository, CachedItemRepository>();
        //builder.Services.AddMemoryCache();
        //builder.Services.AddCaching();
        
        builder.Host.UseSerilogWithSeq();
        builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTemplateMapper>();
        var app = builder.Build();

        //app.UseExceptionHandling();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAngularUI");
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
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
