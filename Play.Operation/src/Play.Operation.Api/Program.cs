using Play.Common.MassTransit;
using Play.Common.Messaging;
using Play.Operation.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMessaging();
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<PlayHub>("/playHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.Run();
