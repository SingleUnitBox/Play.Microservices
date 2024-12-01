using System.Reflection;
using Play.Common.Auth;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.User.Service.Auth;
using Play.User.Service.Context;
using Play.User.Service.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUserService(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddMongo(builder.Configuration)
    .AddMongoRepository<User>("users");
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
