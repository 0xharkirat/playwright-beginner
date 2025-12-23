using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace Playwright.Tests;

public class UnitTest1 : PageTest
{
    [Fact]
    public async Task HasTitle()
    {

        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
    }

    [Fact]
    public async Task GetStartedLink()
    {

        // click the get started link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Get started" }).ClickAsync();

        // Expects page to have a heading with the name of Installation
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Installation" })).ToBeVisibleAsync();

    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        // Additional setup can be done here
        Console.WriteLine("Navigating to Playwright homepage...");
        await Page.GotoAsync("https://playwright.dev/");
    }

    public override async Task DisposeAsync()
    {
        // Additional teardown can be done here
        Console.WriteLine("Test completed. Cleaning up...");
        await base.DisposeAsync();

    }
}
