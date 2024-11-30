using System.Reflection;
using GreenPipes.Caching;
using MassTransit;
using MassTransit.Definition;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Common.Settings;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Consumer;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddMongo(builder.Configuration)
    .AddMongoRepository<InventoryItem>("inventoryItems")
    .AddMongoRepository<CatalogItem>("catalogItems");
    
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(end =>
{
    end.MapControllers();
});

app.Run();

static IServiceCollection AddCatalogClient(IServiceCollection services)
{
    Random jitterer = new Random();
    services.AddHttpClient<CatalogClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5002");
        })
        .AddTransientHttpErrorPolicy(policy =>
            policy.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                5,
                retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
                onRetry: (outcome, timeSpan, retryAttempt) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<CatalogClient>>() 
                        ?.LogWarning($"Delaying for {timeSpan.TotalSeconds} seconds before retrying attempt {retryAttempt}"); 
                }
            ))
        .AddTransientHttpErrorPolicy(policy =>
            policy.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                3,
                TimeSpan.FromSeconds(15),
                onBreak: (outcome, timespan) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<CatalogClient>>() 
                        ?.LogWarning($"Opening to circuit for {timespan.TotalSeconds} seconds...");
                },
                onReset: () =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetService<ILogger<CatalogClient>>() 
                        ?.LogWarning($"Closing to circuit...");
                }
            ))
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
    
    return services;
}
