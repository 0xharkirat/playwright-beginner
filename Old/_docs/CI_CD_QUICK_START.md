# 🚀 Quick Start Guide - GitHub Actions CI/CD

This guide helps you get your GitHub Actions CI/CD pipeline up and running quickly.

## 📋 Prerequisites

Before the workflows can run, ensure you have:

- ✅ All services build locally without errors
- ✅ UI application runs on port 3000
- ✅ Service-One runs on port 5001
- ✅ Service-Two runs on port 5002
- ✅ Playwright tests pass locally

## 🎯 What Was Created?

### Workflow Files (Modular Design)
```
.github/workflows/
├── main.yml           # 🎬 Orchestrator (main pipeline)
├── service-one.yml    # 🔨 Build & run Service-One
├── service-two.yml    # 🔨 Build & run Service-Two
├── ui-app.yml         # 🔨 Build & run Next.js UI
├── playwright.yml     # 🎭 Run Playwright tests with coverage
├── README.md          # 📖 Detailed documentation
└── ARCHITECTURE.md    # 🎨 Visual diagrams & architecture
```

### Test Project Updates
```
tests/Playwright.Tests/
├── Playwright.Tests.csproj  # ✅ Added coverlet.collector package
└── (test files remain unchanged)
```

## ⚡ Quick Test Locally

### 1. Verify Services Can Start

```bash
# Terminal 1: Start Service-One
cd src/Service-One
dotnet run

# Terminal 2: Start Service-Two
cd src/Service-Two
dotnet run

# Terminal 3: Start UI
cd ui
npm install
npm run dev
```

### 2. Verify Tests Pass

```bash
# Terminal 4: Run Playwright tests
cd tests/Playwright.Tests
dotnet test
```

### 3. Verify Coverage Collection Works

```bash
cd tests/Playwright.Tests

# Run tests with coverage
dotnet test \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults

# Check coverage file was generated
ls TestResults/**/coverage.cobertura.xml
```

## 🚀 Deploy to GitHub

### Step 1: Commit Workflow Files

```bash
cd /Users/hark/ssw/playwright-beginner

# Check what's new
git status

# Add workflow files
git add .github/workflows/
git add tests/Playwright.Tests/Playwright.Tests.csproj

# Commit
git commit -m "feat: Add modular GitHub Actions CI/CD pipeline with coverage"

# Push to GitHub
git push origin main
```

### Step 2: Watch Pipeline Run

1. Go to your GitHub repository
2. Click **Actions** tab
3. You'll see **🎭 E2E Test Pipeline** running
4. Click on the run to see the beautiful dependency graph!

### Step 3: View Results

**Pipeline Graph:**
```
Service-One ─┐
Service-Two ─┤─── All Ready ──> Playwright Tests ──> Summary
UI App ──────┘
```

**Artifacts Available:**
- 📊 Coverage Report (HTML)
- ✅ Test Results (TRX format)
- 🎬 Traces (if tests fail)

## 🎨 What the GitHub UI Looks Like

### Actions Tab
```
🎭 E2E Test Pipeline
│
├── 🔨 Service-One (1m 20s) ✅
├── 🔨 Service-Two (1m 15s) ✅
├── 🔨 UI (1m 50s) ✅
│
└── After all services ready:
    ├── 🎭 Playwright Tests (2m 30s) ✅
    └── ✅ Pipeline Complete (5s) ✅
```

### Pull Request Checks
```
✅ All checks have passed

🎭 E2E Test Pipeline
├── Service-One ✅
├── Service-Two ✅
├── UI ✅
├── Playwright Tests ✅
└── Pipeline Complete ✅

📊 Code Coverage: 85.7%
📝 4/4 tests passed
```

## 🔧 Common Issues & Fixes

### Issue 1: Workflow Doesn't Trigger

**Symptom:** Pipeline doesn't run after push

**Fix:**
```bash
# Make sure you pushed the .github/workflows/ directory
git push origin main

# Check workflow file is in the main/develop branch
git ls-tree -r main --name-only | grep .github/workflows/
```

### Issue 2: Services Don't Start in CI

**Symptom:** Health check fails

**Possible Causes:**
1. Service takes longer to start in CI (resource constraints)
2. Port conflicts
3. Missing environment variables

**Fix:** Increase health check timeout in workflow files:

```yaml
# In service-one.yml, service-two.yml, ui-app.yml
- name: Wait for Service to be ready
  run: |
    timeout 120 bash -c '...'  # Increase from 60 to 120 seconds
```

### Issue 3: Coverage Report Not Generated

**Symptom:** Artifact is missing or empty

**Fix:**
```bash
# Locally, verify Coverlet is installed
cd tests/Playwright.Tests
dotnet list package | grep coverlet

# If missing, add it
dotnet add package coverlet.collector

# Commit and push
git add tests/Playwright.Tests/Playwright.Tests.csproj
git commit -m "fix: Ensure coverlet.collector is installed"
git push
```

### Issue 4: Tests Pass Locally but Fail in CI

**Common Reasons:**
1. **Timing issues:** CI is slower than local machine
2. **Missing dependencies:** Playwright browsers not installed
3. **Race conditions:** Services not fully initialized

**Fix:**
```csharp
// In your test files, use explicit waits
await Expect(Page.Locator("selector"))
    .ToBeVisibleAsync(new() { Timeout = 10000 }); // 10 second timeout

// Or update global timeout in appsettings.json
"Timeout": 60000  // Increase from 30000 to 60000
```

## 📊 Viewing Coverage Reports

### After Workflow Completes:

1. Go to workflow run page
2. Scroll to **Artifacts** section
3. Download `coverage-report.zip`
4. Unzip and open `index.html` in browser

**You'll See:**
- Overall coverage percentage
- Line coverage by file
- Branch coverage
- Uncovered lines highlighted in red
- Historical trends

### In Pull Request Comments:

The coverage summary is automatically added as a comment:

```markdown
## Code Coverage Summary

| Module | Line Coverage | Branch Coverage |
|--------|---------------|-----------------|
| Total  | 85.7%        | 78.3%          |
```

## 🎯 What Happens on Each Push?

```
1. You push code to main/develop
   ↓
2. GitHub Actions triggers main.yml
   ↓
3. Three services build & start in parallel
   ↓
4. Health checks ensure all services are ready
   ↓
5. Playwright tests run against live services
   ↓
6. Coverlet collects code coverage during tests
   ↓
7. ReportGenerator creates HTML report
   ↓
8. Artifacts uploaded (test results, coverage, traces)
   ↓
9. Summary posted to GitHub UI and PR comments
   ↓
10. ✅ Pipeline complete!
```

## 🔐 Adding Secrets (For Production)

If your services need API keys or connection strings:

1. Go to **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Add your secrets (e.g., `DATABASE_URL`, `API_KEY`)

Then use in workflows:

```yaml
env:
  DATABASE_URL: ${{ secrets.DATABASE_URL }}
  API_KEY: ${{ secrets.API_KEY }}
```

## 🎉 Next Steps

Now that your CI/CD pipeline is set up:

1. ✅ **Write more tests** - Add to `tests/Playwright.Tests/Tests/`
2. ✅ **Improve coverage** - Aim for 80%+ code coverage
3. ✅ **Add more services** - Create new `service-*.yml` workflow files
4. ✅ **Enable branch protection** - Require CI to pass before merging
5. ✅ **Set up staging deployment** - Add deployment jobs after tests pass

## 📚 Documentation Reference

- **[README.md](README.md)** - Detailed documentation of all workflows
- **[ARCHITECTURE.md](ARCHITECTURE.md)** - Visual diagrams and architecture
- **[Main Project README](../../README.md)** - Project overview and setup

## 💡 Pro Tips

### Tip 1: Manual Workflow Trigger

You can trigger the pipeline manually:
```
GitHub → Actions → 🎭 E2E Test Pipeline → Run workflow
```

### Tip 2: View Past Runs

All workflow runs are saved for 90 days:
```
GitHub → Actions → Click any past run → See full logs
```

### Tip 3: Download Logs

If you need to debug:
```
Workflow run → Top right → Download log archive
```

### Tip 4: Retry Failed Jobs

If a job fails due to transient issues:
```
Workflow run → Re-run failed jobs button
```

## 🚨 Important Notes

⚠️ **GitHub Actions Minutes:** Free tier includes 2,000 minutes/month
- Each run takes ~5-6 minutes
- ~400 runs/month on free tier
- Upgrade for more minutes if needed

⚠️ **Artifact Storage:** Free tier includes 500MB
- Coverage reports: ~5MB each
- Test results: ~1MB each
- Traces: ~10MB each (only on failure)
- Clean up old artifacts periodically

⚠️ **Concurrent Jobs:** Free tier allows 20 concurrent jobs
- Should be more than enough for this setup

## ✅ Success Checklist

Before considering the setup complete, verify:

- [ ] All workflow files committed and pushed
- [ ] Pipeline runs successfully on GitHub
- [ ] All three services start correctly
- [ ] Playwright tests pass
- [ ] Coverage report is generated
- [ ] Artifacts are uploaded
- [ ] PR checks show green status
- [ ] Coverage summary appears in PR comments

## 🎊 You're All Set!

Your CI/CD pipeline is now production-ready! Every push will:
- Build and test all services
- Run comprehensive E2E tests
- Generate coverage reports
- Provide quick feedback on PR health

**Happy Testing! 🎭**
