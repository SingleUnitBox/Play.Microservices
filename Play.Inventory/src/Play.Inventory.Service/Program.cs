using Play.Common.Auth;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.Settings;
using Play.Inventory.Domain.Policies;
using Play.Inventory.Infra;
using Play.Inventory.Infra.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPolicies();
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTempleMapper>();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
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
