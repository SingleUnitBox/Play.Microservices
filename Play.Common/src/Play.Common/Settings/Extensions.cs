using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Play.Common.Settings;

public static class Extensions
{
    public static TSettings GetSettings<TSettings>(this IServiceCollection services, string sectionName)
        where TSettings : class, new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var settings = configuration.GetSettings<TSettings>(sectionName);
        
        return settings;
    }

    public static TSettings GetSettings<TSettings>(this IConfiguration configuration, string sectionName)
        where TSettings : class, new()
    {
        var settings = new TSettings();
        var section = configuration.GetSection(sectionName);
        section.Bind(settings);
        
        return settings;
    }
}