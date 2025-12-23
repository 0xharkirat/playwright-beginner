# Industry-Grade Playwright Test Framework

## 🏗️ Architecture

```
tests/Playwright.Tests/
├── Configuration/
│   ├── TestConfiguration.cs         # Configuration loader
│   └── PlaywrightSettings.cs        # Typed settings model
├── Tests/
│   └── HelloPageTests.cs           # Actual test files
├── BaseTest.cs                      # Base class with custom configuration
├── appsettings.json                 # Single source of truth
└── Playwright.Tests.csproj
```

## 🎯 Design Philosophy

### **1. PageTest (Official Playwright Pattern)**
```csharp
public abstract class BaseTest : PageTest
```

**What PageTest Handles Automatically:**
- ✅ Creates `Playwright` instance
- ✅ Launches browser (per test or shared)
- ✅ Creates fresh `Page` for each test
- ✅ Cleans up after tests (closes browser/pages)
- ✅ Supports parallel execution
- ✅ Takes screenshots on failure

### **2. Our Custom BaseTest Layer**

We extend PageTest to add:
- **Configuration Management** (appsettings.json)
- **Browser Customization** (headless, viewport, etc.)
- **Helper Methods** (NavigateToAsync, TakeScreenshotAsync)
- **Video/Screenshot Setup**

```csharp
public override BrowserNewContextOptions ContextOptions()
{
    return new BrowserNewContextOptions
    {
        BaseURL = Settings.BaseUrl,
        ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
        RecordVideoDir = "videos/",
        // ... more options
    };
}
```

## ⚙️ Configuration Options
- Single Source of Truth

### **appsettings.json** (The ONLY config file you need)
```json
{
  "PlaywrightSettings": {
    "BaseUrl": "http://localhost:3000",
    "BrowserName": "chromium",
    "Headless": true,
    "SlowMo": 0,
    "Timeout": 30000,
    "Screenshot": "only-on-failure",
    "Video": "retain-on-failure",
    "Trace": "on-first-retry"
  }
}
```

**Configuration Flow:**
```
appsettings.json (edit this to change settings)
        ↓
TestConfiguration.cs (loads and caches config)
        ↓
PlaywrightTests (uses appsettings.json)**
```bash
dotnet test
```

### **Run in Headed Mode (see the browser)**
Edit `appsettings.json`:
```json
{
  "PlaywrightSettings": {
    "Headless": false  // Change to false
  }
}
```

Or override with environment variable:
```bash
export PlaywrightSettings__Headless=false
dotnet test
```

### **Run Specific Test**
```bash
dotnet test --filter "FullyQualifiedName~HelloPageTests"
dotnet test --filter "FullyQualifiedName~HelloPage_ShouldLoad"
```

### **Run with Different Browser**
Edit `appsettings.json` or use environment variables:
```bash
export PlaywrightSettings__BrowserName=firefox
dotnet test

export PlaywrightSettings__BrowserName=webkit
dotnet test
```

### **Different Environment URLs**
```bash
# Override BaseUrl for staging
export PlaywrightSettings__BaseUrl=https://staging.example.com
dotnet test

# Override for production
export PlaywrightSettings__BaseUrl=https://prod.example.com


### **Run Specific Test**
```bash
dotnet test --filter "FullyQualifiedName~HelloPageTests"
```

### **Run with Different Browser**
```bash
BROWSER=firefox dotnet test
BROWSER=webkit dotnet test
```

## 📝 Writing Tests

### **Simple Test Example**
```csharp
public class HelloPageTests : BaseTest
{
    [Fact]
    public async Task HelloPage_ShouldLoad()
    {
        // Page is already available from PageTest!
        await NavigateToAsync("/hello");
        
        await Expect(Page.GetByRole(AriaRole.Paragraph))
            .ToContainTextAsync("Hello");
    }
}ingle Source of Truth**
- Only `appsettings.json` for configuration
- No confusion between multiple config files
- Easy to understand and maintain

✅ **Separation of Concerns**
- Configuration in `Configuration/` folder
- Tests in `Tests/` folder
- Base functionality in `BaseTest.cs`

✅ **No Manual Browser Management**
- PageTest handles all lifecycle
- Automatic cleanup
- Parallel execution ready

✅ **Environment-Specific Config**
- One `appsettings.json` for defaults
- Environment variables override for CI/CD
- Optional: `appsettings.Development.json`, `appsettings.Staging.json`

✅ **Strongly Typed Configuration**
- `PlaywrightSettings.cs` provides IntelliSense
- Compile-time checking of property names
- Easy refactoring

✅ **Reusability**
- BaseTest can be copied to other projects
- Configuration pattern is standard .NET
- Helper methods reduce code duplication

✅ **Debugging Support**
- Change `Headless: false` to see browser
- Automatically saved to `test-results/` folder
- Configured in `ContextOptions()`

### **Videos on Failure**
- Recorded only when tests fail
- Saved to `videos/` folder

### **Tracing for Debugging**
- Trace files capture full test execution
- Open with: `pwsh bin/Debug/net9.0/playwright.ps1 show-trace trace.zip`

## 🏭 Industry Best Practices

✅ **Separation of Concerns**
- Configuration in separate folder
- Tests in dedicated folder
- Base functionality in BaseTest

✅ **No Manual Browser Management**
- PageTest handles all lifecycle
- Automatic cleanup
- Parallel execution ready

✅ **Environment-Specific Config**
- `appsettings.json` for URLs, timeouts
- `playwright.runsettings` for browser settings
- Environment variables for CI/CD

✅ **Reusability**
- BaseTest can be copied to other projects
- Configuration pattern is portable
- Helper methods reduce code duplication

✅ **Debugging Support**
- Run in headHow To |
|----------|--------|
| Quick test run | `dotnet test` |
| Debug visually | Set `"Headless": false` in appsettings.json |
| Run one test | `dotnet test --filter "TestName"` |
| CI/CD pipeline | Use environment variables to override settings |
| Different browser | Set `"BrowserName": "firefox"` in appsettings.json |
| Different URL | Set `"BaseUrl": "https://staging.com"` or use env var
```
1. dotnet test starts
   ↓
2. PageTest.InitializeAsync()
   - Creates Playwright instance
   - Launches browser (Chromium/Firefox/WebKit)
   ↓
3. ContextOptions() called (our customization)
   - Sets BaseURL, viewport, video recording
   ↓
4. PageTest creates new Page for test
   ↓
5. Your test method [Fact] runs

### How do I change configuration for CI/CD?
Use environment variables (standard .NET pattern):
```bash
export PlaywrightSettings__Headless=true
export PlaywrightSettings__BaseUrl=https://production.com
dotnet test
```

## 📋 Environment Variables Override Pattern

The `__` (double underscore) syntax is the .NET standard for nested configuration:

```bash
# Format: Section__PropertyName=value
export PlaywrightSettings__BaseUrl=https://example.com
export PlaywrightSettings__Headless=true
export PlaywrightSettings__BrowserName=firefox
export PlaywrightSettings__Timeout=60000
```

This overrides values in `appsettings.json` without editing the file!

---
   - Page is ready to use!
   - await NavigateToAsync("/hello")
   - await Expect(...).ToBeVisibleAsync()
   ↓
6. PageTest.DisposeAsync()
   - Saves video if test failed
   - Takes screenshot if test failed
   - Closes page and browser
```

## 📦 VS Code Test Explorer

Tests appear in VS Code Test Explorer because:
1. ✅ Project has `Microsoft.Playwright.Xunit` package
2. ✅ Tests have `[Fact]` attribute
3. ✅ Project is in solution file
4. ✅ C# Dev Kit extension is installed

**Run tests from:**
- Test Explorer sidebar (beaker icon)
- Code lens above test methods
- Command Palette: "Test: Run All Tests"

## 🎯 When to Use What

| Scenario | Tool |
|----------|------|
| Quick test run | `dotnet test` |
| Debug visually | `HEADED=1 dotnet test` |
| Run one test | `dotnet test --filter` |
| CI/CD pipeline | `dotnet test --settings playwright.runsettings` |
| Different browser | `BROWSER=firefox dotnet test` |

## 🔍 Troubleshooting

### Browsers not installed?
```bash
pwsh bin/Debug/net9.0/playwright.ps1 install
```

### Tests not appearing in VS Code?
1. Reload window: `Cmd+Shift+P` → "Developer: Reload Window"
2. Build project: `dotnet build`
3. Check C# Dev Kit is installed

### Want to see browser?
Set `"Headless": false` in appsettings.json or use `HEADED=1`

This framework is production-ready and can be copied to any .NET + Playwright project! 🎉
