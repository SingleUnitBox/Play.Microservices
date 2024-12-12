using Play.Common.Auth;
using Play.Common.Context;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.User.Core.Auth;
using Play.User.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUserService(builder.Configuration);
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddContext();
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddMongoRepository<IUserRepository, UserMongoRepository>(
    db => new UserMongoRepository(db, "users"));
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());

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
