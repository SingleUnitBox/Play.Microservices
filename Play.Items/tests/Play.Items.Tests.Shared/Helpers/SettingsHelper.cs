using Microsoft.Extensions.Configuration;

namespace Play.Items.Tests.Shared.Helpers;

public class SettingsHelper
{
    private const string AppSettings = "appsettings.test.json";
    
    public static TSettings GetSettings<TSettings>()
        where TSettings : class, new()
    {
        var settings = new TSettings();
        var configuration = GetConfiguration();
        configuration.GetSection(nameof(TSettings)).Bind(settings);
        
        return settings;
    }
    
    public static TSettings GetSettings<TSettings>(string sectionName)
        where TSettings : class, new()
    {
        var settings = new TSettings();
        var configuration = GetConfiguration();
        configuration.GetSection(sectionName).Bind(settings);
        
        return settings;
    }
    
    private static IConfigurationRoot GetConfiguration()
        => new ConfigurationBuilder()
            .AddJsonFile(AppSettings)
            .AddEnvironmentVariables()
            .Build();
}