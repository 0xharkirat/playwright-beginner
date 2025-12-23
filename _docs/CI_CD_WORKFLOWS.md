# GitHub Actions CI/CD Pipeline

This directory contains the modular GitHub Actions workflows for our monorepo project.

## 🎯 Architecture Overview

The CI/CD pipeline follows the **Orchestrator Pattern** with reusable workflows for modularity and maintainability.

### Pipeline Flow

```
main.yml (Orchestrator)
├── 🔨 Stage 1: Build & Start Services (Parallel)
│   ├── service-one.yml → Build & run Service-One on port 5001
│   ├── service-two.yml → Build & run Service-Two on port 5002
│   └── ui-app.yml → Build & run Next.js UI on port 3000
│
├── 🎭 Stage 2: Run E2E Tests (Sequential - after all services ready)
│   └── playwright.yml → Run Playwright tests with coverage
│
└── ✅ Stage 3: Summary (Report pipeline status)
```

## 📁 Workflow Files

### 1. `main.yml` - Orchestrator Workflow
**Purpose:** Main pipeline that coordinates all jobs and creates dependency graph

**Triggers:**
- Push to `main` or `develop` branches
- Pull requests to `main` or `develop`
- Manual dispatch (`workflow_dispatch`)

**Features:**
- Concurrency control (cancels in-progress runs)
- Parallel service builds for speed
- Sequential test execution after services ready
- Pipeline summary report

**Graph Visualization:** GitHub Actions UI shows a nice dependency graph with all stages

### 2. `service-one.yml` - Service-One Workflow
**Purpose:** Build and run Service-One microservice

**Type:** Reusable workflow (`workflow_call`)

**Steps:**
1. Checkout code
2. Setup .NET 9.0
3. Restore dependencies
4. Build in Release mode
5. Start service on port 5001 (background)
6. Health check (waits for Swagger endpoint)
7. Export service URL as output

**Outputs:** `service-url` (http://localhost:5001)

### 3. `service-two.yml` - Service-Two Workflow
**Purpose:** Build and run Service-Two microservice

**Type:** Reusable workflow (`workflow_call`)

**Steps:**
1. Checkout code
2. Setup .NET 9.0
3. Restore dependencies
4. Build in Release mode
5. Start service on port 5002 (background)
6. Health check (waits for Swagger endpoint)
7. Export service URL as output

**Outputs:** `service-url` (http://localhost:5002)

### 4. `ui-app.yml` - UI Application Workflow
**Purpose:** Build and run Next.js UI application

**Type:** Reusable workflow (`workflow_call`)

**Steps:**
1. Checkout code
2. Setup Node.js 20 with npm caching
3. Install dependencies (`npm ci`)
4. Build Next.js app
5. Start app on port 3000 (background)
6. Health check (waits for HTTP 200)
7. Export app URL as output

**Outputs:** `app-url` (http://localhost:3000)

### 5. `playwright.yml` - Playwright Tests Workflow
**Purpose:** Run E2E tests with code coverage reporting

**Type:** Reusable workflow (`workflow_call`)

**Inputs:**
- `service-one-url`: Service-One URL
- `service-two-url`: Service-Two URL
- `ui-url`: UI application URL

**Steps:**
1. Checkout code
2. Setup .NET 9.0
3. Restore & build test project
4. Install Playwright browsers with dependencies
5. Update `appsettings.json` with service URLs
6. Run tests with Coverlet coverage collector
7. Generate HTML coverage report with ReportGenerator
8. Upload test results as artifacts
9. Upload coverage report as artifacts
10. Upload traces (on failure)
11. Publish test summary to GitHub UI

**Artifacts:**
- `playwright-test-results` (30 days retention)
- `coverage-report` (30 days retention)
- `playwright-traces` (7 days retention - only on failure)

**Coverage Tools:**
- **Coverlet:** Collects code coverage data
- **ReportGenerator:** Generates HTML and Markdown reports

## 🚀 How to Use

### Running Locally

You can test individual workflows locally using [act](https://github.com/nektos/act):

```bash
# Install act (macOS)
brew install act

# Run the full pipeline
act -W .github/workflows/main.yml

# Run only Playwright tests (requires services running)
act -W .github/workflows/playwright.yml
```

### Viewing Results in GitHub UI

1. **Pipeline Graph:** Go to Actions tab → Select workflow run → View dependency graph
2. **Test Results:** Click on test job → See test summary in job output
3. **Coverage Report:** Download `coverage-report` artifact → Open `index.html`
4. **Traces:** On test failure, download `playwright-traces` → Open with Playwright Trace Viewer

### Manual Trigger

1. Go to **Actions** tab in GitHub
2. Select **🎭 E2E Test Pipeline**
3. Click **Run workflow**
4. Choose branch and click **Run workflow**

## 📊 Code Coverage

The pipeline uses **Coverlet** to collect code coverage during test execution:

```bash
dotnet test \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
```

**ReportGenerator** then creates:
- **HTML Report:** Interactive coverage report with drill-down
- **Markdown Summary:** Added to GitHub Actions summary and PR comments

### Coverage Report Contents

- Line coverage percentages
- Branch coverage percentages
- File-by-file coverage breakdown
- Uncovered lines highlighting
- Historical trends (if enabled)

## 🔧 Configuration

### Modifying Service Ports

Edit the respective workflow file:

```yaml
# service-one.yml
env:
  ASPNETCORE_URLS: http://localhost:5001  # Change port here
```

### Changing Browser for Tests

Update `appsettings.json` in the Playwright workflow:

```yaml
- name: Update appsettings.json
  run: |
    cat > appsettings.json << EOF
    {
      "PlaywrightSettings": {
        "BrowserName": "firefox",  # chromium, firefox, webkit
        "Headless": true
      }
    }
    EOF
```

### Adjusting Test Timeout

Modify the `timeout-minutes` in `playwright.yml`:

```yaml
jobs:
  test:
    timeout-minutes: 30  # Change this value
```

## 🐛 Troubleshooting

### Services Not Starting

**Symptom:** Tests fail because services are unreachable

**Solution:** Check health check timeout in service workflows:

```yaml
- name: Wait for Service-One to be ready
  run: |
    timeout 60 bash -c 'until curl -f http://localhost:5001/swagger > /dev/null 2>&1; do sleep 2; done'
```

Increase timeout from 60 to 120 seconds if services are slow to start.

### Tests Failing in CI but Passing Locally

**Potential Causes:**
1. **Race conditions:** Services not fully initialized
2. **Resource constraints:** GitHub runners have limited CPU/memory
3. **Timing differences:** Network latency in CI

**Solutions:**
- Increase test timeouts in `appsettings.json`
- Add explicit waits in tests using `await Expect().ToBeVisibleAsync()`
- Enable video recording to debug: `"Video": "on"`

### Coverage Report Not Generated

**Check:**
1. Coverlet package is installed: `dotnet add package coverlet.collector`
2. Coverage files exist in TestResults: Check workflow logs
3. ReportGenerator installed correctly: Look for installation errors

## 🎯 Best Practices

### ✅ Do's

- **Keep workflows modular:** Each service has its own reusable workflow
- **Use outputs:** Pass data between workflows (service URLs)
- **Fail fast:** Set appropriate timeouts for jobs
- **Cache dependencies:** Use `cache: 'npm'` for Node.js, implicit for .NET restore
- **Retain artifacts:** Keep important artifacts (test results, coverage) for 30 days
- **Add summaries:** Use `$GITHUB_STEP_SUMMARY` for readable reports

### ❌ Don'ts

- **Avoid hardcoded secrets:** Use GitHub Secrets for sensitive data
- **Don't run tests in parallel if they share state:** Playwright tests should be isolated
- **Avoid long-running jobs:** Break them into smaller jobs with checkpoints
- **Don't ignore failures:** Always check job status and propagate failures

## 📚 Resources

- [GitHub Actions Reusable Workflows](https://docs.github.com/en/actions/using-workflows/reusing-workflows)
- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator Documentation](https://github.com/danielpalme/ReportGenerator)
- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [GitHub Actions Best Practices](https://docs.github.com/en/actions/learn-github-actions/workflow-syntax-for-github-actions)

## 🔮 Future Enhancements

- [ ] Add Docker Compose for service orchestration
- [ ] Implement matrix strategy for multi-browser testing
- [ ] Add performance testing with k6
- [ ] Integrate Allure reporting for richer test reports
- [ ] Add deployment workflows for staging/production
- [ ] Implement parallel test execution with test sharding
- [ ] Add security scanning with dependency checks
