using Asp.Versioning;
using Play.Common.Auth;
using Play.Common.Logging;
using Play.Common.Logging.Mappers;
using Play.Common.Observability;
using Play.Common.Settings;
using Play.Items.Infra;
using Play.Items.Infra.Logging;
using Play.Items.Infra.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Host.UseSerilogWithSeq();
builder.Services.AddSingleton<IMessageToLogTemplateMapper, MessageToLogTemplateMapper>();
var app = builder.Build();

//app.UseExceptionHandling();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Services.GetRequiredService<ItemsMetrics>();
app.UseCors("AllowAngularUI");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<CustomMetricsMiddleware>();
// app.UsePlayMetrics();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", (ctx) =>
    {
        var settings = ctx.RequestServices
            .GetService<IConfiguration>()
            .GetSettings<ServiceSettings>(nameof(ServiceSettings));
        
        return ctx.Response.WriteAsJsonAsync($"Hello from Play.{settings.ServiceName}.Service");
    });
    endpoints.MapGet("/file", async () =>
    {
        string filePath = "C:\\Users\\czlom\\source\\repos\\Play.Microservices\\Play.Catalog\\static\\file.txt";

        var fileContents = await File.ReadAllTextAsync(filePath);
        return Results.Text(fileContents, "text/plain");
    });
});
#pragma warning restore ASP0014

app.Run();
