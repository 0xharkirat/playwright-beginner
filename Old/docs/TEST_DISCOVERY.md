# Why VS Code Test Explorer Picks Up C# Playwright Tests

## Test Discovery in VS Code

Your C# xUnit Playwright tests will be automatically discovered by VS Code through multiple mechanisms:

### 1. **C# Dev Kit / C# Extension**
- Automatically discovers xUnit test projects
- Shows tests in the Test Explorer sidebar
- Requires: [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) or [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)

### 2. **Solution File Recognition**
- The `playwright-beginner.sln` file at the root includes all .NET projects
- VS Code recognizes the solution and all referenced test projects
- Test Explorer automatically scans projects with test frameworks

### 3. **Test Project Markers**
Your `Playwright.Tests.csproj` has these indicators that mark it as a test project:
```xml
<IsPackable>false</IsPackable>
<PackageReference Include="xunit" />
<PackageReference Include="xunit.runner.visualstudio" />
<PackageReference Include="Microsoft.NET.Test.Sdk" />
```

### 4. **xUnit Test Attributes**
Tests are marked with `[Fact]` attributes:
```csharp
[Fact]
public async Task HomePage_ShouldLoad()
{
    // Test code
}
```

## How to View and Run Tests

### Test Explorer Panel
1. Open the Test Explorer: Click the beaker icon in the Activity Bar (left sidebar)
2. You'll see all tests under `Playwright.Tests`
3. Click the play button next to any test to run it

### Code Lens
- Test methods show "Run Test | Debug Test" links above them
- Click to run individual tests directly from the editor

### Command Palette
- `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac)
- Type "Test: Run All Tests" or "Test: Run Test at Cursor"

## Why Not TypeScript Playwright Tests?

TypeScript Playwright tests use a different discovery mechanism:
- Requires the Playwright Test VS Code extension
- Uses `playwright.config.ts` file
- Runs through Node.js test runner

**You chose C# xUnit because:**
1. ✅ **Native .NET integration** - Tests can reference your .NET services directly
2. ✅ **Same solution file** - All projects (services + tests) in one place
3. ✅ **Familiar testing patterns** - xUnit is standard for .NET developers
4. ✅ **Better IntelliSense** - Full C# type safety and autocomplete
5. ✅ **Test Explorer integration** - Same UI for all .NET tests
6. ✅ **CI/CD simplicity** - One build pipeline for all .NET code

## Troubleshooting

### Tests Not Appearing?
1. Ensure C# Dev Kit is installed
2. Reload window: `Ctrl+Shift+P` → "Developer: Reload Window"
3. Build the test project: `dotnet build tests/Playwright.Tests/Playwright.Tests.csproj`
4. Check Output panel → "C# Dev Kit" for errors

### Playwright Browsers Not Installed?
```bash
cd tests/Playwright.Tests
dotnet build
pwsh bin/Debug/net9.0/playwright.ps1 install
```

### Tests Fail to Run?
- Make sure services are running (Service-One on 5001, Service-Two on 5002, UI on 3000)
- Or update test URLs to match your actual ports
