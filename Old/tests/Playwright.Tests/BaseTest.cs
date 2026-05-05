using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Playwright.Tests.Configuration;

namespace Playwright.Tests;

/// <summary>
/// Industry-grade base test class that extends PageTest.
/// 
/// ARCHITECTURE:
/// 1. PageTest handles browser lifecycle automatically
/// 2. We customize browser options via BrowserNewContextOptions
/// 3. Configuration is loaded from appsettings.json
/// 4. Each test gets a fresh browser context (isolation)
/// 
/// BENEFITS:
/// - Automatic setup/teardown (no manual InitializeAsync/DisposeAsync needed)
/// - Browser configuration from appsettings.json
/// - Screenshots/videos on failure
/// - Tracing for debugging
/// - Parallel test execution support
/// </summary>
public abstract class BaseTest : PageTest
{
    private static PlaywrightSettings Settings => TestConfiguration.PlaywrightSettings;
    protected static string BaseUrl => Settings.BaseUrl;

    /// <summary>
    /// Called once before creating the browser context.
    /// Override to customize browser options.
    /// </summary>
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = Settings.BaseUrl,
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },

            // Video recording (only on failure by default)
            RecordVideoDir = Settings.Video != "off" ? "videos/" : null,
            RecordVideoSize = new RecordVideoSize { Width = 1920, Height = 1080 },

            // Locale and timezone
            Locale = "en-US",
            TimezoneId = "America/New_York"
        };
    }

    /// <summary>
    /// Navigate to a path relative to the configured BaseUrl
    /// </summary>
    protected async Task NavigateToAsync(string path = "/")
    {
        var url = BaseUrl.TrimEnd('/') + path;
        await Page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });
    }

    /// <summary>
    /// Take a screenshot with a custom name
    /// </summary>
    protected async Task TakeScreenshotAsync(string name)
    {
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
            FullPage = true
        });
    }
}
