using Play.Common.Commands;
using Play.Common.Context;
using Play.Common.Events;
using Play.Common.Logging;
using Play.Common.RabbitMq;
using Play.Operation.Api.Hubs;
using Play.Operation.Api.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddCommands();
builder.Services.AddEvents();
//builder.Services.AddLoggingEventHandlerDecorator();
builder.Services.AddRabbitMq()
    .AddCommandConsumer()
    .AddEventConsumer();
builder.Services.AddScoped<IOperationStatusService, OperationStatusService>();

builder.Host.UseSerilogWithSeq();
var app = builder.Build();

app.UseRouting();
app.MapHub<PlayHub>("/playHub");
app.UseEndpoints(e => e.MapControllers());
app.UseStaticFiles();
app.Run();
