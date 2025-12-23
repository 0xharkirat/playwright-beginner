namespace Playwright.Tests.Configuration;

/// <summary>
/// Playwright-specific settings loaded from appsettings.json
/// </summary>
public class PlaywrightSettings
{
    public string BaseUrl { get; set; } = "http://localhost:3000";
    public string BrowserName { get; set; } = "chromium";
    public bool Headless { get; set; } = true;
    public int SlowMo { get; set; } = 0;
    public int Timeout { get; set; } = 30000;
    public string Screenshot { get; set; } = "only-on-failure";
    public string Video { get; set; } = "retain-on-failure";
    public string Trace { get; set; } = "on-first-retry";
}
