using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Playwright.Tests.Tests;

public class HelloPageTests : BaseTest
{
    [Fact]
    public async Task HelloPage_ShouldDisplay_HelloMessage_InParagraph()
    {
        // Arrange & Act
        await NavigateToAsync("/hello");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert - Check if "Hello" text is visible in a paragraph
        var paragraph = Page.GetByRole(AriaRole.Paragraph);
        await Expect(paragraph).ToBeVisibleAsync();
        await Expect(paragraph).ToContainTextAsync("Hello World");
    }


}
