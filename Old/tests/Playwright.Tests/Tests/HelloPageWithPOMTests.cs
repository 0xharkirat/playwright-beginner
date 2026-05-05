using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Playwright.Tests.Pages;

namespace Playwright.Tests.Tests;

/// <summary>
/// EXAMPLE: Tests using Page Object Model pattern.
/// 
/// This file shows HOW to use POM, but for a simple Hello page,
/// the direct approach (see HelloPageTests.cs) is actually better!
/// 
/// Use POM when:
/// - You have 5+ tests for the same page
/// - The page has complex interactions
/// - You're testing a real application with many pages
/// </summary>
public class HelloPageWithPOMTests : BaseTest
{
    [Fact]
    public async Task HelloPage_ShouldDisplay_HelloMessage_UsingPOM()
    {
        // Arrange - Create page object
        var helloPage = new HelloPage(Page, BaseUrl);

        // Act
        await helloPage.GotoAsync();

        // Assert - Using the page object's exposed locators
        await Expect(helloPage.ParagraphLocator).ToBeVisibleAsync();
        await Expect(helloPage.ParagraphLocator).ToContainTextAsync("Hello World");
    }

    [Fact]
    public async Task HelloPage_ShouldHave_ParagraphText_UsingPOM()
    {
        // Arrange
        var helloPage = new HelloPage(Page, BaseUrl);

        // Act
        await helloPage.GotoAsync();

        // Assert - ✅ Use Expect (auto-waits, retries, better errors)
        await Expect(helloPage.ParagraphLocator).ToContainTextAsync("Hello World");
        await Expect(helloPage.ParagraphLocator).Not.ToBeEmptyAsync();
    }

    [Fact]
    public async Task HelloPage_ShouldLoad_Quickly_UsingPOM()
    {
        // Arrange
        var helloPage = new HelloPage(Page, BaseUrl);

        // Act
        await helloPage.GotoAsync();

        // Assert - ✅ Use Expect instead of manual waiting
        await Expect(helloPage.ParagraphLocator).ToBeVisibleAsync();
        await Expect(helloPage.ParagraphLocator).ToContainTextAsync("Hello World");
    }
}

/* 
 * ✅ BEST PRACTICE: Always use Expect() instead of Assert()
 * 
 * WHY Expect() over Assert():
 * ✅ Auto-waits (up to 5 seconds by default)
 * ✅ Auto-retries until condition is met
 * ✅ Better error messages with screenshots
 * ✅ Web-first assertions designed for UI testing
 * ✅ Official Playwright pattern
 * 
 * WRONG ❌:
 * var text = await element.TextContentAsync();
 * Assert.Contains("Hello", text);  // No waiting, fails immediately
 * 
 * RIGHT ✅:
 * await Expect(element).ToContainTextAsync("Hello");  // Waits + retries
 * 
 * ---
 * 
 * COMPARISON: WITHOUT POM vs WITH POM
 * 
 * WITHOUT POM (current HelloPageTests.cs):
 * - 5 lines of code per test
 * - Easy to understand
 * - Perfect for simple pages
 * 
 * WITH POM (this file):
 * - 3 lines of code per test (more reusable)
 * - Selectors defined once in HelloPage.cs
 * - Better for complex pages with many tests
 * 
 * RECOMMENDATION FOR THIS PROJECT:
 * Keep the simple approach (no POM) until you have:
 * - 10+ test files
 * - Repeated selectors across tests
 * - Complex page interactions
 */
