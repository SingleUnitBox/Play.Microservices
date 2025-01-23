using Play.Common.Events;
using Play.Common.RabbitMq;
using Play.Operation.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddEvents();
builder.Services.AddRabbitMq()
    .AddEventConsumer();

var app = builder.Build();

app.UseRouting();
app.MapHub<PlayHub>("/playHub");
app.UseEndpoints(e => e.MapControllers());
app.UseStaticFiles();
app.Run();
