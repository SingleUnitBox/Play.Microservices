using System.Reflection;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Common.Settings;


namespace Play.Catalog.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, Assembly.GetExecutingAssembly());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        
        builder.Services.AddMongoDb(builder.Configuration);
        builder.Services.AddMongoRepository<IItemRepository, ItemRepository>(
            db => new ItemRepository(db, "items"));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}