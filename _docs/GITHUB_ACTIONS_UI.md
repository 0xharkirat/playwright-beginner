# 🎯 GitHub Actions Dependency Graph Visualization

When you view your workflow run in the GitHub Actions UI, you'll see a beautiful dependency graph like this:

## 📊 Workflow Graph (What You'll See in GitHub UI)

```
┌─────────────────────────────────────────────────────────────┐
│                     🎭 E2E Test Pipeline                    │
│                                                             │
│  Trigger: Push to main/develop, Pull Request, Manual       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
        ┌───────────────────────────────────────┐
        │   🔨 Stage 1: Build & Start Services  │
        │         (Parallel Execution)          │
        └───────────────────────────────────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
┌─────────────┐ ┌─────────────┐ ┌─────────────┐
│ Service-One │ │ Service-Two │ │   UI App    │
│             │ │             │ │             │
│  • Setup    │ │  • Setup    │ │  • Setup    │
│  • Build    │ │  • Build    │ │  • Install  │
│  • Start    │ │  • Start    │ │  • Build    │
│  • Health✅ │ │  • Health✅ │ │  • Start    │
│             │ │             │ │  • Health✅ │
│ Output:     │ │ Output:     │ │ Output:     │
│ 5001        │ │ 5002        │ │ 3000        │
└─────────────┘ └─────────────┘ └─────────────┘
        │           │           │
        └───────────┼───────────┘
                    │
                    ▼
        ┌───────────────────────────────────────┐
        │ 🎭 Stage 2: Run E2E Tests             │
        │      (Sequential Execution)           │
        └───────────────────────────────────────┘
                    │
                    ▼
        ┌─────────────────────────┐
        │   Playwright Tests      │
        │                         │
        │  • Install browsers     │
        │  • Run tests            │
        │  • Collect coverage     │
        │  • Generate reports     │
        │                         │
        │  Artifacts:             │
        │  ├─ Test Results 📊     │
        │  ├─ Coverage Report 📈  │
        │  └─ Traces (if fail) 🎬 │
        └─────────────────────────┘
                    │
                    ▼
        ┌───────────────────────────────────────┐
        │    ✅ Stage 3: Pipeline Summary       │
        │                                       │
        │  • Check all jobs status              │
        │  • Post summary to PR                 │
        │  • Update GitHub checks               │
        └───────────────────────────────────────┘
```

## 🎨 Color Coding in GitHub UI

When you view this in GitHub Actions:

- 🟢 **Green**: Job completed successfully
- 🟡 **Yellow**: Job is currently running
- 🔴 **Red**: Job failed
- ⚪ **Gray**: Job waiting to run or skipped
- 🔵 **Blue**: Job queued

## ⏱️ Timeline View

```
Time →
0:00  ┌─ Service-One starts
      ├─ Service-Two starts  
      └─ UI starts
      
1:30  ├─ Service-One ready ✅
      ├─ Service-Two ready ✅
      └─ (UI still building...)
      
2:00  └─ UI ready ✅
      
2:01  ┌─ Playwright Tests start
      
4:30  ├─ Tests complete ✅
      ├─ Coverage generated 📊
      └─ Artifacts uploaded
      
4:35  └─ Pipeline Summary ✅

Total Time: ~4-5 minutes
```

## 📱 Mobile View

On mobile devices, GitHub will show a simplified list:

```
🎭 E2E Test Pipeline
├─ 🔨 Service-One (1m 30s) ✅
├─ 🔨 Service-Two (1m 25s) ✅
├─ 🔨 UI (2m 00s) ✅
├─ 🎭 Playwright Tests (2m 30s) ✅
└─ ✅ Pipeline Summary (5s) ✅

Status: Success
Duration: 4m 35s
```

## 🔍 Drill-Down View

When you click on any job, you'll see detailed steps:

### Example: Service-One Job

```
Service-One
│
├─ 📥 Checkout code (3s) ✅
├─ 🔧 Setup .NET (12s) ✅
├─ 📦 Restore dependencies (8s) ✅
├─ 🔨 Build Service-One (15s) ✅
├─ 🚀 Start Service-One in background (2s) ✅
├─ ⏳ Wait for Service-One to be ready (5s) ✅
├─ 📤 Set output (1s) ✅
└─ 💾 Save PID for cleanup (1s) ✅
```

### Example: Playwright Tests Job

```
Playwright Tests
│
├─ 📥 Checkout code (3s) ✅
├─ 🔧 Setup .NET (12s) ✅
├─ 📦 Restore test dependencies (10s) ✅
├─ 🔨 Build test project (15s) ✅
├─ 🌐 Install Playwright browsers (45s) ✅
├─ ⚙️ Update appsettings.json (1s) ✅
├─ 🧪 Run tests with coverage (60s) ✅
├─ 📊 Generate coverage report (10s) ✅
├─ 💬 Add coverage to PR comment (2s) ✅
├─ 📤 Upload test results (5s) ✅
├─ 📤 Upload coverage report (8s) ✅
└─ 📝 Publish test summary (3s) ✅
```

## 🎭 Status Badges

You can add these to your README:

```markdown
![CI/CD Pipeline](https://github.com/YOUR_USERNAME/playwright-beginner/workflows/E2E%20Test%20Pipeline/badge.svg)
```

**Displays:**
- ✅ Passing (green badge)
- ❌ Failing (red badge)
- 🟡 Running (yellow badge)

## 📊 Pull Request Integration

When you create a PR, you'll see:

```
┌─────────────────────────────────────────────┐
│ All checks have passed                      │
├─────────────────────────────────────────────┤
│ ✅ E2E Test Pipeline / Service-One          │
│ ✅ E2E Test Pipeline / Service-Two          │
│ ✅ E2E Test Pipeline / UI                   │
│ ✅ E2E Test Pipeline / Playwright Tests     │
│ ✅ E2E Test Pipeline / Pipeline Complete    │
├─────────────────────────────────────────────┤
│ 📊 Code Coverage                            │
│ ├─ Line Coverage: 85.7% ↑ +2.3%            │
│ ├─ Branch Coverage: 78.3% ↑ +1.1%          │
│ └─ 4/4 tests passed                         │
├─────────────────────────────────────────────┤
│ 🎯 Artifacts                                │
│ ├─ 📊 Coverage Report (5.2 MB)              │
│ ├─ ✅ Test Results (1.1 MB)                 │
│ └─ View full details →                      │
└─────────────────────────────────────────────┘
```

## 🚦 Commit Status Checks

Next to each commit, you'll see status icons:

```
abc1234 feat: Add new feature
        🟢 All checks passed (4m 35s ago)

def5678 fix: Update service config
        🔴 Some checks failed (2h ago)
        
ghi9012 docs: Update README
        🟡 Checks running... (Running for 2m)
```

## 🎯 Job Dependencies Visualization

```
Main Pipeline
│
├─── needs: (none) ──┐
│                    ├─ Service-One
│                    ├─ Service-Two
│                    └─ UI
│
└─── needs: [Service-One, Service-Two, UI] ──┐
                                             ├─ Playwright Tests
                                             │
                                             └─ needs: [Playwright Tests]
                                                └─ Pipeline Summary
```

## 📈 Workflow Insights

GitHub provides analytics:

```
Insights → Actions

📊 Workflow Runs (Last 30 Days)
├─ Total Runs: 87
├─ Success Rate: 94.3%
├─ Average Duration: 4m 42s
├─ Fastest Run: 3m 15s
└─ Slowest Run: 6m 30s

📈 Trends
├─ Success rate improving ↑
├─ Duration stable →
└─ 5 runs per day (avg)
```

## 🎨 Dark Mode vs Light Mode

GitHub Actions UI supports both themes:

**Dark Mode:**
- Background: Dark gray (#0d1117)
- Success: Green (#238636)
- Failure: Red (#da3633)
- Running: Yellow (#bb8009)

**Light Mode:**
- Background: White (#ffffff)
- Success: Green (#1a7f37)
- Failure: Red (#cf222e)
- Running: Yellow (#9a6700)

## 🔔 Notifications

You'll receive notifications when:
- ✅ Workflow completes successfully
- ❌ Workflow fails
- 🔄 Re-run requested
- 💬 Someone mentions you in workflow comments

Configure at: **Settings** → **Notifications** → **Actions**

## 🎊 Success View Example

```
┌────────────────────────────────────────────────────┐
│  🎉 Workflow completed successfully                │
│                                                    │
│  E2E Test Pipeline #42                             │
│  main branch • abc1234                             │
│  Triggered by: push                                │
│  Duration: 4m 35s                                  │
│                                                    │
│  Jobs:                                             │
│  ├─ Service-One (1m 30s) ✅                        │
│  ├─ Service-Two (1m 25s) ✅                        │
│  ├─ UI (2m 00s) ✅                                 │
│  ├─ Playwright Tests (2m 30s) ✅                   │
│  └─ Pipeline Summary (5s) ✅                       │
│                                                    │
│  Artifacts: 3                                      │
│  ├─ 📊 Coverage Report (5.2 MB)                    │
│  ├─ ✅ Test Results (1.1 MB)                       │
│  └─ View all artifacts →                           │
│                                                    │
│  [Re-run Jobs] [View YAML] [...]                   │
└────────────────────────────────────────────────────┘
```

## 🎯 Quick Actions

From the workflow run page:

- **Re-run all jobs** - Run everything again
- **Re-run failed jobs** - Only re-run what failed
- **Cancel workflow** - Stop running workflow
- **View YAML** - See the workflow configuration
- **Download logs** - Get full execution logs
- **Download artifacts** - Get test results/coverage

## 💡 Pro Tip

Pin frequently viewed workflows to the top:
```
Actions → Select workflow → ⭐ Pin
```

This keeps important workflows easily accessible!
