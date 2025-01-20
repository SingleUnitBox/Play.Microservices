using MassTransit;
using Play.Common.Abs.Commands;
using Play.Common.Abs.Events;
using Play.Common.AppInitializer;
using Play.Common.Auth;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.Messaging;
using Play.Common.PostgresDb;
using Play.Common.PostgresDb.UnitOfWork.Decorators;
using Play.Common.Queries;
using Play.Common.Settings;
using Play.Inventory.Domain.Policies;
using Play.Inventory.Infra.Consumer.Events.Items;
using Play.Inventory.Infra.Logging;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Repositories;
using Play.Inventory.Infra.Postgres.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<AppInitializer>();
builder.Services.AddExceptionHandling();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

//builder.Services.AddMongoDb(builder.Configuration);
//builder.Services.AddMongoRepositories();
builder.Services.AddPostgresDb<InventoryPostgresDbContext>();
builder.Services.AddPostgresRepositories();
builder.Services.AddMassTransit((configure) =>
{
    configure.AddConsumer<ItemCreatedConsumer>();
    configure.UsingRabbitMq((ctx, config) =>
    {
        var rabbitMqSettings = builder.Configuration.GetSettings<RabbitMqSettings>(nameof(RabbitMqSettings));
        config.Host(rabbitMqSettings.Host);
        config.Publish<IEvent>(p => p.Exclude = true);
        config.ReceiveEndpoint("ItemCreated", e => e.ConfigureConsumer<ItemCreatedConsumer>(ctx));
        
        config.ConfigureEndpoints(ctx);
    });
});
//auto registered now?
//builder.Services.AddMassTransitHostedService();

builder.Services.AddPolicies();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddMessaging();
builder.Services.AddQueries();
builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTempleMapper>();
builder.Services.AddLoggingQueryHandlerDecorator();
builder.Services.AddCommands();
builder.Services.AddLoggingCommandHandlerDecorator();
builder.Services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
builder.Services.AddEvents();
builder.Services.AddLoggingEventHandlerDecorator();

builder.Services.AddAPostgresCommandHandlerDecorator();
builder.Services.AddPostgresUnitOfWork<IInventoryUnitOfWork, InventoryPostgresUnitOfWork>();

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
#pragma warning disable ASP0014
app.UseEndpoints(end =>
{
    end.MapGet("/", (ctx) =>
    {
        var serviceSettings = ctx.RequestServices
        .GetService<IConfiguration>()
        .GetSettings<ServiceSettings>(nameof(ServiceSettings));
        
        return ctx.Response.WriteAsJsonAsync($"Hello from Play.{serviceSettings.ServiceName}.Service");
    });
    end.MapControllers();
});
#pragma warning restore ASP0014

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


// ItemCreatedNameFromAttribute
//     Routing Key	
//     Redelivered	●
// Properties	
// message_id:	49d10000-b882-08bf-329b-08dd38df2335
// correlation_id:	00000000-0000-0000-0000-000000000000
// delivery_mode:	2
// headers:	
// Content-Type:	application/vnd.masstransit+json
// publishId:	7
// content_type:	application/vnd.masstransit+json
// Payload
// 1026 bytes
// Encoding: string
// {
//
//     "messageId": "49d10000-b882-08bf-329b-08dd38df2335",
//
//     "correlationId": "00000000-0000-0000-0000-000000000000",
//
//     "conversationId": "49d10000-b882-08bf-4406-08dd38df2331",
//
//     "initiatorId": "14983d63-07ea-4f3e-9a3e-db959428d0e6",
//
//     "sourceAddress": "rabbitmq://localhost:0/CreateItemCommand",
//
//     "destinationAddress": "rabbitmq://localhost:0/ItemCreatedNameFromAttribute",
//
//     "messageType": [
//
//     "urn:message:Play.Items.Contracts.Events:ItemCreated",
//
//     "urn:message:Play.Common.Abs.Events:IEvent"
//
//         ],
//
//     "message": {
//
//         "itemId": "667b9fc4-a610-4deb-ad37-c50f5c8c2427",
//
//         "name": "XXL Potion",
//
//         "price": "66.99"
//
//     },
//
//     "sentTime": "2025-01-19T23:15:17.6029851Z",
//
//     "headers": {},
//
//     "host": {
//
//         "machineName": "CZ",
//
//         "processName": "Play.Items.Api",
//
//         "processId": 17052,
//
//         "assembly": "Play.Items.Api",
//
//         "assemblyVersion": "1.0.0.0",
//
//         "frameworkVersion": "8.0.11",
//
//         "massTransitVersion": "7.3.1.0",
//
//         "operatingSystemVersion": "Microsoft Windows NT 10.0.22631.0"
//
//     }
//
// }
