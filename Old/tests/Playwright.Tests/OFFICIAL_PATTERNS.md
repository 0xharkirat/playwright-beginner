# Official Playwright Patterns & Best Practices

## 📚 What the Official Docs Say

### 1. Configuration Pattern (From Official Docs)

**Official Approach: Override `ContextOptions()`**

The official Playwright .NET docs recommend overriding `ContextOptions()` method in your base test class:

```csharp
public class ExampleTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            ColorScheme = ColorScheme.Light,
            ViewportSize = new() { Width = 1920, Height = 1080 },
            BaseURL = "https://github.com"
        };
    }
}
```

**Our Enhancement: Add appsettings.json**

We extended this pattern by:
- ✅ Loading settings from `appsettings.json` (industry standard)
- ✅ Keeping the official `ContextOptions()` override
- ✅ Making configuration reusable across tests

**Result: Best of Both Worlds**
```csharp
// Our pattern (combines official pattern + .NET config)
public abstract class BaseTest : PageTest
{
    private static PlaywrightSettings Settings => TestConfiguration.PlaywrightSettings;
    
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            BaseURL = Settings.BaseUrl,  // From appsettings.json
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }
        };
    }
}
```

### 2. Browser/Launch Configuration

**Official Methods (from docs):**

**Option A: Use .runsettings file (optional)**
```xml
<Playwright>
  <BrowserName>chromium</BrowserName>
  <LaunchOptions>
    <Headless>false</Headless>
  </LaunchOptions>
</Playwright>
```

**Option B: Command line**
```bash
dotnet test -- Playwright.BrowserName=chromium Playwright.LaunchOptions.Headless=false
```

**Option C: Environment variables**
```bash
BROWSER=chromium dotnet test
HEADED=1 dotnet test
```

**Our Approach:**
We use `appsettings.json` instead of `.runsettings` because:
- ✅ Simpler (one config file vs multiple)
- ✅ Standard .NET Core pattern
- ✅ Works with environment variables (`PlaywrightSettings__Headless=false`)
- ✅ Easier for developers familiar with .NET

---

## 🏗️ Page Object Model (Official Pattern)

### What the Official Docs Say

**From [playwright.dev/dotnet/docs/pom](https://playwright.dev/dotnet/docs/pom):**

> "Page object models are one approach to structure your test suite. Page objects **simplify authoring** by creating a higher-level API which suits your application and **simplify maintenance** by capturing element selectors in one place."

### Official POM Implementation

```csharp
// SearchPage.cs (Page Object)
public class SearchPage
{
    private readonly IPage _page;
    private readonly ILocator _searchTermInput;

    public SearchPage(IPage page)
    {
        _page = page;
        _searchTermInput = page.Locator("[aria-label='Enter your search term']");
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://bing.com");
    }

    public async Task SearchAsync(string text)
    {
        await _searchTermInput.FillAsync(text);
        await _searchTermInput.PressAsync("Enter");
    }
}

// Usage in test
var searchPage = new SearchPage(Page);
await searchPage.GotoAsync();
await searchPage.SearchAsync("playwright");
```

### When to Use POM (Official Guidance)

✅ **Use POM when:**
- You have **large test suites**
- Same UI elements used across **multiple tests**
- Need to **reduce duplication**
- Want **easier maintenance** (change selector in one place)

❌ **Don't use POM when:**
- You have a **small test suite** (adds unnecessary complexity)
- Testing simple flows
- Just getting started (start simple, refactor later)

---

## 🎯 Recommended Structure

### For Small Projects (What We Have Now)
```
tests/Playwright.Tests/
├── Configuration/
│   ├── TestConfiguration.cs
│   └── PlaywrightSettings.cs
├── Tests/
│   └── HelloPageTests.cs
├── BaseTest.cs
└── appsettings.json
```

**Pros:**
- ✅ Simple and straightforward
- ✅ Easy to understand
- ✅ Quick to get started
- ✅ Perfect for learning

### For Large Projects (Add Page Objects)
```
tests/Playwright.Tests/
├── Configuration/
│   ├── TestConfiguration.cs
│   └── PlaywrightSettings.cs
├── Pages/                      # Page Objects
│   ├── HomePage.cs
│   ├── LoginPage.cs
│   └── CheckoutPage.cs
├── Tests/
│   ├── LoginTests.cs
│   ├── CheckoutTests.cs
│   └── SearchTests.cs
├── BaseTest.cs
└── appsettings.json
```

**Pros:**
- ✅ Scales to hundreds of tests
- ✅ Selectors in one place
- ✅ Reusable page actions
- ✅ Easier to maintain

---

## 📊 Comparison: Our Pattern vs Official Examples

| Aspect | Official Docs | Our Implementation | Verdict |
|--------|---------------|-------------------|---------|
| Base Class | `PageTest` | `BaseTest : PageTest` | ✅ Same |
| Configuration | `.runsettings` or hardcoded | `appsettings.json` | ✅ Better (standard .NET) |
| Context Options | Override `ContextOptions()` | Override `ContextOptions()` | ✅ Same |
| Browser Control | Env vars or CLI | `appsettings.json` + env vars | ✅ Same flexibility |
| POM Support | Recommended for large suites | Can be added when needed | ✅ Same guidance |

---

## 🚀 Evolution Path

### Phase 1: Simple Tests (Current - Perfect for Learning)
```csharp
public class HelloPageTests : BaseTest
{
    [Fact]
    public async Task HelloPage_ShouldDisplay_HelloMessage()
    {
        await NavigateToAsync("/hello");
        await Expect(Page.GetByRole(AriaRole.Paragraph)).ToBeVisibleAsync();
    }
}
```

**When to move to Phase 2:** When you have 5+ test files using the same UI elements

### Phase 2: Add Page Objects (For Growing Projects)
```csharp
// Pages/HelloPage.cs
public class HelloPage
{
    private readonly IPage _page;
    private readonly ILocator _paragraph;

    public HelloPage(IPage page)
    {
        _page = page;
        _paragraph = page.GetByRole(AriaRole.Paragraph);
    }

    public async Task GotoAsync(string baseUrl)
    {
        await _page.GotoAsync($"{baseUrl}/hello");
    }

    public ILocator Paragraph => _paragraph;
}

// Tests/HelloPageTests.cs
public class HelloPageTests : BaseTest
{
    [Fact]
    public async Task HelloPage_ShouldDisplay_HelloMessage()
    {
        var helloPage = new HelloPage(Page);
        await helloPage.GotoAsync(BaseUrl);
        await Expect(helloPage.Paragraph).ToBeVisibleAsync();
    }
}
```

**When to move to Phase 3:** When you have complex multi-page workflows

### Phase 3: App/Workflow Objects (For Enterprise)
```csharp
// Workflows/CheckoutWorkflow.cs
public class CheckoutWorkflow
{
    private readonly HomePage _homePage;
    private readonly ProductPage _productPage;
    private readonly CartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;

    public CheckoutWorkflow(IPage page)
    {
        _homePage = new HomePage(page);
        _productPage = new ProductPage(page);
        _cartPage = new CartPage(page);
        _checkoutPage = new CheckoutPage(page);
    }

    public async Task CompleteCheckoutAsync(string productName, PaymentInfo payment)
    {
        await _homePage.SearchAsync(productName);
        await _productPage.AddToCartAsync();
        await _cartPage.ProceedToCheckoutAsync();
        await _checkoutPage.CompleteOrderAsync(payment);
    }
}
```

---

## ✅ Our Current Implementation Verdict

**Is it official?** ✅ Yes
- Uses official `PageTest` base class
- Follows official `ContextOptions()` override pattern
- Supports official browser configuration methods

**Is it industry-standard?** ✅ Yes
- Uses .NET Core configuration pattern (`appsettings.json`)
- Supports environment variables
- Follows separation of concerns

**Is it production-ready?** ✅ Yes
- Scales from 1 test to 1000s of tests
- Can add Page Objects when needed
- Works in CI/CD pipelines

**Should we add Page Objects now?** ❌ No
- Current project is small
- Would add unnecessary complexity
- Start simple, refactor when needed

---

## 📖 References

- [Official Playwright .NET Docs - Test Runners](https://playwright.dev/dotnet/docs/test-runners)
- [Official Playwright .NET Docs - Page Object Models](https://playwright.dev/dotnet/docs/pom)
- [Microsoft Learn - Playwright Training](https://learn.microsoft.com/en-us/training/modules/build-with-playwright/)

**Bottom Line:** Our implementation follows official Playwright patterns PLUS industry-standard .NET configuration. It's exactly what you'd see in a professional codebase! 🎉
