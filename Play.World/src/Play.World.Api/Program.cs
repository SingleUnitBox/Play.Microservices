using Play.Common.Logging;
using Play.World.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseSerilogWithSeq();

var app = builder.Build();
app.UseCors("AllowAll");
app.UseRouting();
app.MapControllers();
app.Run();