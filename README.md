# Playwright Beginner - Monorepo

A monorepo containing .NET microservices, Next.js UI, and Playwright E2E tests written in C# with xUnit.

## Project Structure

```
playwright-beginner/
├── .github/
│   └── workflows/           # CI/CD pipelines
│       ├── main.yml        # Orchestrator workflow
│       ├── service-one.yml # Service-One build & run
│       ├── service-two.yml # Service-Two build & run
│       ├── ui-app.yml      # UI build & run
│       └── playwright.yml  # E2E tests with coverage
├── _docs/                   # 📚 Comprehensive documentation
│   ├── README.md           # Documentation index
│   ├── CI_CD_QUICK_START.md
│   ├── CI_CD_WORKFLOWS.md
│   ├── WORKFLOW_ARCHITECTURE.md
│   ├── GITHUB_ACTIONS_UI.md
│   └── IMPLEMENTATION_SUMMARY.md
├── src/                     # Production application code
│   ├── Service-One/        # .NET 9.0 Web API
│   └── Service-Two/        # .NET 9.0 Web API
├── tests/                   # Test projects
│   └── Playwright.Tests/   # C# xUnit Playwright E2E tests
│       ├── README.md       # Test framework guide
│       └── OFFICIAL_PATTERNS.md
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

### 🧪 Test Framework Documentation

- **[Test Framework Guide](tests/Playwright.Tests/README.md)** - Architecture and usage
- **[Official Patterns Guide](tests/Playwright.Tests/OFFICIAL_PATTERNS.md)** - Playwright patterns explained

### Test Configuration

- **Single source of truth:** `appsettings.json`
- **BaseTest class:** Extends PageTest with custom configuration
- **Page Object Model:** Optional pattern for complex pages (see examples)
- **Coverage:** Integrated with Coverlet for code coverage reporting

## CI/CD

The project uses a **modular GitHub Actions pipeline** with orchestrator pattern:

- **[main.yml](.github/workflows/main.yml)**: Orchestrator workflow coordinating all jobs
- **[service-one.yml](.github/workflows/service-one.yml)**: Build & run Service-One
- **[service-two.yml](.github/workflows/service-two.yml)**: Build & run Service-Two
- **[ui-app.yml](.github/workflows/ui-app.yml)**: Build & run Next.js UI
- **[playwright.yml](.github/workflows/playwright.yml)**: E2E tests with Coverlet coverage

### Pipeline Flow

```
Stage 1: Build Services (Parallel)
   ├─ Service-One (port 5001)
   ├─ Service-Two (port 5002)
   └─ UI (port 3000)
      ↓
Stage 2: Run E2E Tests
   └─ Playwright Tests + Coverage
      ↓
Stage 3: Publish Results
   └─ Test results, Coverage reports, Traces
```

### 📚 CI/CD Documentation

Comprehensive guides available in the [`_docs/`](_docs/) directory:

- **[🚀 Quick Start Guide](_docs/CI_CD_QUICK_START.md)** - Get CI/CD running in 5 minutes
- **[📖 Workflow Documentation](_docs/CI_CD_WORKFLOWS.md)** - Detailed workflow reference
- **[🎨 Architecture & Diagrams](_docs/WORKFLOW_ARCHITECTURE.md)** - Visual architecture guide
- **[👁️ GitHub UI Guide](_docs/GITHUB_ACTIONS_UI.md)** - What to expect in GitHub
- **[✅ Implementation Summary](_docs/IMPLEMENTATION_SUMMARY.md)** - Complete overview

### Key Features

✅ Modular reusable workflows  
✅ Parallel service builds (saves ~60% time)  
✅ Code coverage with HTML reports  
✅ Beautiful dependency graph in GitHub UI  
✅ PR integration with test results  
✅ Trace uploads on test failures

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
- **_docs/**: Comprehensive documentation in one place
- **Solution file at root**: VS Code and Visual Studio can discover all .NET projects
- **C# Playwright with xUnit**: 
  - Native integration with .NET Test Explorer
  - Can reference and test .NET services directly
  - Better type safety and IntelliSense
  - Familiar testing patterns for .NET developers
- **Modular CI/CD**: 
  - Reusable workflows for each service
  - Parallel execution for speed
  - Beautiful dependency graph in GitHub UI

## 📚 Documentation

This project has comprehensive documentation covering all aspects:

### For Developers

- **[Getting Started Guide](_docs/CI_CD_QUICK_START.md)** - Set up CI/CD in 5 minutes
- **[Test Framework Guide](tests/Playwright.Tests/README.md)** - Write and run tests
- **[Workflow Documentation](_docs/CI_CD_WORKFLOWS.md)** - Understand CI/CD pipelines

### For DevOps/Architects

- **[Workflow Architecture](_docs/WORKFLOW_ARCHITECTURE.md)** - Visual diagrams and architecture
- **[Official Patterns](tests/Playwright.Tests/OFFICIAL_PATTERNS.md)** - Test patterns explained
- **[Implementation Summary](_docs/IMPLEMENTATION_SUMMARY.md)** - Complete overview

### For Everyone

- **[📚 Documentation Index](_docs/README.md)** - Central hub for all documentation
- **[GitHub Actions UI Guide](_docs/GITHUB_ACTIONS_UI.md)** - What to expect in GitHub

## 🚀 Quick Links

- [CI/CD Quick Start](_docs/CI_CD_QUICK_START.md) - Get running in 5 minutes
- [Test Framework Guide](tests/Playwright.Tests/README.md) - Write your first test
- [Documentation Index](_docs/README.md) - Browse all documentation
