using Microsoft.Playwright;

namespace Playwright.Tests;

public class ExampleTests : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private IBrowserContext? _context;
    private IPage? _page;

    public async Task InitializeAsync()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await _page?.CloseAsync()!;
        await _context?.CloseAsync()!;
        await _browser?.CloseAsync()!;
        _playwright?.Dispose();
    }

    [Fact]
    public async Task HomePage_ShouldLoad()
    {
        // Navigate to the Next.js UI
        await _page!.GotoAsync("http://localhost:3000");
        
        // Wait for page to load
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Assert page title or content
        var title = await _page.TitleAsync();
        Assert.NotEmpty(title);
    }

    [Fact]
    public async Task ServiceOne_HealthCheck_ShouldReturn200()
    {
        // Test Service-One health endpoint
        var response = await _page!.GotoAsync("http://localhost:5001/swagger");
        Assert.True(response?.Ok);
    }

    [Fact]
    public async Task ServiceTwo_HealthCheck_ShouldReturn200()
    {
        // Test Service-Two health endpoint
        var response = await _page!.GotoAsync("http://localhost:5002/swagger");
        Assert.True(response?.Ok);
    }
}
