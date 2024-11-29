using MassTransit;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;
using Play.Common.MongoDb;
using Play.Common.Settings;

namespace Play.Catalog.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        
        builder.Services.AddMongo(builder.Configuration)
            .AddMongoRepository<Item>("items");
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMqSettings = builder.Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
                cfg.Host(rabbitMqSettings.Host);
                cfg.ConfigureEndpoints(ctx, 
                    new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                
            });
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

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