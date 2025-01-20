namespace Play.APIGateway.Commands;

public static class Extensions
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMqClient>(sp =>
            new RabbitMqClient(sp.GetRequiredService<IConfiguration>()));
        services.AddSingleton<CommandPublisher>();
        
        return services;
    }
}