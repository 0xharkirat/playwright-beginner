using Microsoft.Extensions.Configuration;

namespace Playwright.Tests.Configuration;

/// <summary>
/// Centralized configuration management for all tests.
/// Loads settings from appsettings.json and environment variables.
/// </summary>
public static class TestConfiguration
{
    private static IConfiguration? _configuration;
    private static PlaywrightSettings? _playwrightSettings;

    public static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .Build();
            }
            return _configuration;
        }
    }

    public static PlaywrightSettings PlaywrightSettings
    {
        get
        {
            if (_playwrightSettings == null)
            {
                _playwrightSettings = new PlaywrightSettings();
                Configuration.GetSection("PlaywrightSettings").Bind(_playwrightSettings);
            }
            return _playwrightSettings;
        }
    }

    /// <summary>
    /// Quick access to BaseUrl
    /// </summary>
    public static string BaseUrl => PlaywrightSettings.BaseUrl;
}
