# Playwright Beginner - Monorepo

A monorepo containing .NET microservices, Next.js UI, and Playwright E2E tests written in C# with xUnit.

## Project Structure

```
playwright-beginner/
├── .github/
│   └── workflows/           # CI/CD pipelines
│       ├── dotnet-services.yml
│       ├── ui.yml
│       └── playwright-tests.yml
├── src/                     # Production application code
│   ├── Service-One/        # .NET 9.0 Web API
│   └── Service-Two/        # .NET 9.0 Web API
├── tests/                   # Test projects
│   └── Playwright.Tests/   # C# xUnit Playwright E2E tests
├── ui/                      # Next.js frontend application
├── .gitignore
├── playwright-beginner.sln  # Solution file containing all .NET projects
└── README.md
```

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [PowerShell Core](https://github.com/PowerShell/PowerShell) (for Playwright browser installation)

### Running the Services

**Service-One (Port 5001):**
```bash
dotnet run --project src/Service-One/Service-One.csproj
```

**Service-Two (Port 5002):**
```bash
dotnet run --project src/Service-Two/Service-Two.csproj
```

### Running the UI

```bash
cd ui
npm install
npm run dev
```

The UI will be available at http://localhost:3000

### Running Playwright Tests

**First time setup - Install Playwright browsers:**
```bash
cd tests/Playwright.Tests
dotnet build
pwsh bin/Debug/net9.0/playwright.ps1 install
```

**Run all tests:**
```bash
dotnet test tests/Playwright.Tests/Playwright.Tests.csproj
```

**Run tests in Visual Studio Code:**
1. Install the [.NET Test Explorer](https://marketplace.visualstudio.com/items?itemName=formulahendry.dotnet-test-explorer) extension
2. Open the Test Explorer panel
3. Click "Run All Tests" or run individual tests

### Building the Solution

Build all .NET projects:
```bash
dotnet build
```

Build specific project:
```bash
dotnet build src/Service-One/Service-One.csproj
```

## Testing Strategy

The Playwright tests are written in C# using xUnit and test the entire system:
- UI functionality (Next.js frontend)
- API endpoints (Service-One and Service-Two)
- End-to-end user flows

Tests are located in `tests/Playwright.Tests/` and can be discovered by:
- Visual Studio Code Test Explorer
- Visual Studio Test Explorer
- `dotnet test` CLI

## CI/CD

GitHub Actions workflows are configured for:
- **dotnet-services.yml**: Builds and tests .NET services
- **ui.yml**: Builds and tests Next.js application
- **playwright-tests.yml**: Runs E2E tests against all services

## Development

### Adding New .NET Projects

```bash
dotnet new webapi -n NewService -o src/NewService
dotnet sln add src/NewService/NewService.csproj
```

### Adding New Test Files

Create new test classes in `tests/Playwright.Tests/` following the xUnit pattern with `IAsyncLifetime` for Playwright setup/teardown.

## Why This Structure?

- **src/**: Clearly separates production code from tests
- **tests/**: Groups all test projects together
- **Solution file at root**: VS Code and Visual Studio can discover all .NET projects
- **C# Playwright with xUnit**: 
  - Native integration with .NET Test Explorer
  - Can reference and test .NET services directly
  - Better type safety and IntelliSense
  - Familiar testing patterns for .NET developers
