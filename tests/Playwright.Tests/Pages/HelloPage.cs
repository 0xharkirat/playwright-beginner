using Microsoft.Playwright;

namespace Playwright.Tests.Pages;

/// <summary>
/// Page Object Model for the Hello page.
/// 
/// WHEN TO CREATE THIS:
/// - When you have 3+ tests using the same page
/// - When selectors are repeated across tests
/// - When page logic becomes complex
/// 
/// FOR NOW: This is just an example - not needed for simple tests!
/// </summary>
public class HelloPage
{
    private readonly IPage _page;
    private readonly string _baseUrl;

    // Locators - defined once, used many times
    private ILocator Paragraph => _page.GetByRole(AriaRole.Paragraph);
    private ILocator Heading => _page.GetByRole(AriaRole.Heading);

    public HelloPage(IPage page, string baseUrl)
    {
        _page = page;
        _baseUrl = baseUrl;
    }

    /// <summary>
    /// Navigate to the Hello page
    /// </summary>
    public async Task GotoAsync()
    {
        await _page.GotoAsync($"{_baseUrl}/hello");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    /// <summary>
    /// Get the paragraph text
    /// </summary>
    public async Task<string?> GetParagraphTextAsync()
    {
        return await Paragraph.TextContentAsync();
    }

    /// <summary>
    /// Check if paragraph contains specific text
    /// </summary>
    public async Task<bool> ContainsTextAsync(string text)
    {
        var content = await GetParagraphTextAsync();
        return content?.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    /// <summary>
    /// Wait for the message to be visible
    /// </summary>
    public async Task WaitForMessageAsync()
    {
        await Paragraph.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        });
    }

    // Expose locators for assertions in tests
    public ILocator ParagraphLocator => Paragraph;
    public ILocator HeadingLocator => Heading;
}
