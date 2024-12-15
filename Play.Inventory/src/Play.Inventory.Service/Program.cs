using Play.Common.Auth;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Exceptions;
using Play.Common.MassTransit;
using Play.Common.PostgresDb;
using Play.Common.Queries;
using Play.Inventory.Domain.Policies;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Repositories;
using Play.Common.AppInitializer;
using Play.Common.MongoDb;
using Play.Inventory.Infra.Queries.Handlers;
using Play.Inventory.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandling();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddMongoRepositories();
//builder.Services.AddPostgresDb<InventoryPostgresDbContext>();
//builder.Services.AddPostgresRepositories();
builder.Services.AddHostedService<AppInitializer>();
builder.Services.AddPolicies();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddQueries();
builder.Services.AddQueryHandlers();
builder.Services.AddCommands();
// builder.Services.AddMassTransit(x =>
// {
//     //x.AddConsumer<CatalogItemCreatedConsumer>();
//     x.AddConsumers(AppDomain.CurrentDomain.GetAssemblies());
//     x.SetKebabCaseEndpointNameFormatter();
//     //x.SetEndpointNameFormatter(new SnakeCaseEndpointNameFormatter("inventory", false));
//
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         var rabbitmqSettings = builder.Configuration
//             .GetSection(nameof(RabbitMqSettings))
//             .Get<RabbitMqSettings>();
//         cfg.Host(rabbitmqSettings.Host);
//         cfg.ConfigureEndpoints(context);s
//
//         // cfg.ReceiveEndpoint("inventory-items", e =>
//         // {
//         //     e.ConfigureConsumer<CatalogItemCreatedConsumer>(context);
//         //
//         //     // Bind to the publisher's exchange
//         //     e.Bind("Play.Items.Application.Events:ItemCreated");
//         // });
//     });
// });
//builder.Services.AddMassTransitHostedService();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandling();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(end =>
{
    end.MapControllers();
});

app.Run();

// static IServiceCollection AddCatalogClient(IServiceCollection services)
// {
//     Random jitterer = new Random();
//     services.AddHttpClient<CatalogClient>(client =>
//         {
//             client.BaseAddress = new Uri("http://localhost:5002");
//         })
//         .AddTransientHttpErrorPolicy(policy =>
//             policy.Or<TimeoutRejectedException>().WaitAndRetryAsync(
//                 5,
//                 retryAttempt => 
//                     TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
//                     + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),
//                 onRetry: (outcome, timeSpan, retryAttempt) =>
//                 {
//                     var serviceProvider = services.BuildServiceProvider();
//                     serviceProvider.GetService<ILogger<CatalogClient>>() 
//                         ?.LogWarning($"Delaying for {timeSpan.TotalSeconds} seconds before retrying attempt {retryAttempt}"); 
//                 }
//             ))
//         .AddTransientHttpErrorPolicy(policy =>
//             policy.Or<TimeoutRejectedException>().CircuitBreakerAsync(
//                 3,
//                 TimeSpan.FromSeconds(15),
//                 onBreak: (outcome, timespan) =>
//                 {
//                     var serviceProvider = services.BuildServiceProvider();
//                     serviceProvider.GetService<ILogger<CatalogClient>>() 
//                         ?.LogWarning($"Opening to circuit for {timespan.TotalSeconds} seconds...");
//                 },
//                 onReset: () =>
//                 {
//                     var serviceProvider = services.BuildServiceProvider();
//                     serviceProvider.GetService<ILogger<CatalogClient>>() 
//                         ?.LogWarning($"Closing to circuit...");
//                 }
//             ))
//         .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
//     
//     return services;
// }
