using System.Reflection;
using Play.Common.Commands;
using Play.Common.Exceptions;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Common.Queries;
using Play.Items.Domain.Repositories;
using Play.Items.Infra.Repositories;

namespace Play.Items.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddExceptionHandling();
        //builder.Services.AddCustomExceptionToResponseMapper<CatalogExceptionMapper>();
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, Assembly.GetExecutingAssembly());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCommands();
        builder.Services.AddQueries();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        
        builder.Services.AddMongoDb(builder.Configuration);
        builder.Services.AddMongoRepository<IItemRepository, ItemRepository>(
            db => new ItemRepository(db, "items"));

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