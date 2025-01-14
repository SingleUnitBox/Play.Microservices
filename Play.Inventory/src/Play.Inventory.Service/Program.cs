using Microsoft.EntityFrameworkCore;
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
using Play.Common.Events;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.Messaging;
using Play.Common.MongoDb;
using Play.Common.Settings;
using Play.Inventory.Infra.Queries.Handlers;
using Play.Inventory.Infra.Repositories;
using Play.Common.Settings;
using Play.Inventory.Infra.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<AppInitializer>();
builder.Services.AddExceptionHandling();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

//builder.Services.AddMongoDb(builder.Configuration);
//builder.Services.AddMongoRepositories();
builder.Services.AddPostgresDb<InventoryPostgresDbContext>();
builder.Services.AddPostgresRepositories();

builder.Services.AddPolicies();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMessaging();
builder.Services.AddQueries();
builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTempleMapper>();
builder.Services.AddLoggingQueryHandlerDecorator();
builder.Services.AddCommands();
builder.Services.AddLoggingCommandHandlerDecorator();
builder.Services.AddEvents();
builder.Services.AddLoggingEventHandlerDecorator();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilogWithSeq();
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
