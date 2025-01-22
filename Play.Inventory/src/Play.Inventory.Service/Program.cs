using Play.Common.Abs.Commands;
using Play.Common.AppInitializer;
using Play.Common.Auth;
using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Exceptions;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.PostgresDb;
using Play.Common.PostgresDb.UnitOfWork.Decorators;
using Play.Common.Queries;
using Play.Common.RabbitMq;
using Play.Common.Settings;
using Play.Inventory.Domain.Policies;
using Play.Inventory.Infra.Logging;
using Play.Inventory.Infra.Postgres;
using Play.Inventory.Infra.Postgres.Repositories;
using Play.Inventory.Infra.Postgres.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<AppInitializer>();
builder.Services.AddExceptionHandling();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddRabbitMq();

//builder.Services.AddMongoDb(builder.Configuration);
//builder.Services.AddMongoRepositories();
builder.Services.AddPostgresDb<InventoryPostgresDbContext>();
builder.Services.AddPostgresRepositories();

builder.Services.AddPolicies();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddQueries();
builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTempleMapper>();
builder.Services.AddLoggingQueryHandlerDecorator();
builder.Services.AddCommands();
builder.Services.AddLoggingCommandHandlerDecorator();
builder.Services.AddEvents();
builder.Services.AddLoggingEventHandlerDecorator();
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

