# 🐳 Docker-Based CI/CD Guide

This guide explains how to use Docker and Docker Compose for local development and CI/CD testing.

## 🎯 Why Docker?

**Problem with background processes in CI:**
- GitHub Actions doesn't maintain background processes between steps reliably
- Services need to stay alive during test execution
- Health checks are difficult to manage with background `&` processes

**Docker Compose solution:**
- All services run in isolated containers on same network
- Services communicate via container names (e.g., `http://service-one:5001`)
- Health checks built into Docker Compose
- Works identically locally and in CI
- Playwright runs in container with access to all services

## 📁 Files Created

```
playwright-beginner/
├── docker-compose.yml              # Orchestrates all services
├── src/
│   ├── Service-One/
│   │   └── Dockerfile             # Service-One container
│   └── Service-Two/
│       └── Dockerfile             # Service-Two container
├── ui/
│   └── Dockerfile                 # Next.js UI container
└── tests/Playwright.Tests/
    └── Dockerfile                 # Playwright test container
```

## 🚀 Quick Start

### Local Development

**Start all services:**
```bash
docker compose up -d service-one service-two ui
```

**Check service health:**
```bash
docker compose ps
```

**Run Playwright tests:**
```bash
# Option 1: Run tests in Docker (recommended)
docker compose run --rm playwright-tests

# Option 2: Run tests locally against Docker services
dotnet test tests/Playwright.Tests/Playwright.Tests.csproj
```

**Stop all services:**
```bash
docker compose down
```

**View logs:**
```bash
docker compose logs service-one
docker compose logs service-two
docker compose logs ui
```

### CI/CD Pipeline

The `.github/workflows/main-docker.yml` workflow:

1. **Build & Validate** (parallel):
   - Service-One build validation
   - Service-Two build validation
   - UI build validation

2. **E2E Tests** (sequential):
   - Build all Docker images
   - Start services with `docker compose up -d`
   - Wait for health checks to pass
   - Run Playwright tests in container
   - Generate coverage reports
   - Upload artifacts

3. **Cleanup**:
   - Show logs if tests fail
   - Stop all services

## 🏗️ Architecture

### Docker Network

```
┌─────────────────────────────────────────┐
│         Docker Network (app-network)    │
│                                         │
│  ┌────────────┐  ┌────────────┐       │
│  │ service-one│  │ service-two│       │
│  │   :5001    │  │   :5002    │       │
│  └────────────┘  └────────────┘       │
│         │              │               │
│         └──────┬───────┘               │
│                │                       │
│          ┌─────▼──────┐                │
│          │     ui     │                │
│          │   :3000    │                │
│          └─────┬──────┘                │
│                │                       │
│      ┌─────────▼──────────┐            │
│      │  playwright-tests  │            │
│      │   (test runner)    │            │
│      └────────────────────┘            │
└─────────────────────────────────────────┘
```

### Service Communication

Services communicate using **container names** as hostnames:

```yaml
# Inside Playwright test container
PlaywrightSettings__BaseUrl=http://ui:3000

# UI can call services:
# http://service-one:5001/api/...
# http://service-two:5002/api/...
```

## ⚙️ Configuration

### Test Configuration for Docker

Update `appsettings.json` for Docker:

```json
{
  "PlaywrightSettings": {
    "BaseUrl": "http://ui:3000",  // Container name, not localhost
    "BrowserName": "chromium",
    "Headless": true
  }
}
```

**Environment variable override** (in docker-compose.yml):
```yaml
environment:
  - PlaywrightSettings__BaseUrl=http://ui:3000
```

### Port Mapping

Docker Compose maps container ports to host:

```yaml
ports:
  - "5001:5001"  # host:container
  - "5002:5002"
  - "3000:3000"
```

**From host machine:**
- Service-One: `http://localhost:5001`
- Service-Two: `http://localhost:5002`
- UI: `http://localhost:3000`

**From within containers:**
- Service-One: `http://service-one:5001`
- Service-Two: `http://service-two:5002`
- UI: `http://ui:3000`

## 🔧 Dockerfile Details

### Service-One & Service-Two Dockerfiles

Multi-stage build:
1. **Build stage**: Uses SDK image, restores, builds, publishes
2. **Runtime stage**: Uses minimal ASP.NET image, copies published output

**Benefits:**
- Smaller final image (~200MB vs ~1GB)
- Only runtime dependencies included
- Faster startup in CI

### UI Dockerfile

Multi-stage build:
1. **Builder stage**: `npm install` + `npm run build`
2. **Production stage**: Only built files + node_modules for production

### Playwright Tests Dockerfile

- Based on .NET SDK image
- Installs PowerShell for Playwright CLI
- Installs Chromium browser with dependencies
- Installs ReportGenerator for coverage
- Runs tests with coverage collection

## 🧪 Testing Scenarios

### 1. Full E2E Tests (Recommended for CI)

```bash
# Start services and run tests
docker compose up -d service-one service-two ui
docker compose run --rm playwright-tests
docker compose down
```

### 2. Iterative Development

```bash
# Start services once
docker compose up -d

# Run tests multiple times (faster)
dotnet test tests/Playwright.Tests/Playwright.Tests.csproj

# Make code changes, rebuild specific service
docker compose up -d --build service-one

# Stop when done
docker compose down
```

### 3. Debug Individual Service

```bash
# Run service with logs visible
docker compose up service-one

# Or attach to running service
docker compose logs -f service-one
```

## 📊 Health Checks

Each service has a health check:

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:5001/swagger/index.html"]
  interval: 10s
  timeout: 5s
  retries: 5
  start_period: 10s
```

**Health check states:**
- `starting`: Service is starting up
- `healthy`: Service passed health check
- `unhealthy`: Service failed health check

**In CI, tests wait for all services to be healthy** before running.

## 🐛 Troubleshooting

### Services not starting

```bash
# Check service logs
docker compose logs service-one
docker compose logs service-two
docker compose logs ui

# Check service status
docker compose ps

# Rebuild images
docker compose build --no-cache
```

### Tests can't reach services

**Issue:** Tests use `localhost` instead of container names

**Fix:** Update `appsettings.json` or use environment variables:
```bash
docker compose run --rm -e PlaywrightSettings__BaseUrl=http://ui:3000 playwright-tests
```

### Port conflicts

**Issue:** Port already in use on host

**Fix:** Stop conflicting service or change port mapping:
```yaml
ports:
  - "5011:5001"  # Map to different host port
```

### Health checks failing

**Issue:** Service takes longer to start than health check allows

**Fix:** Increase `start_period` and `interval`:
```yaml
healthcheck:
  start_period: 30s  # Give more time to start
  interval: 15s      # Check less frequently
```

## 🚀 CI/CD Best Practices

### 1. Use Docker Compose in CI

✅ **Do this:**
```yaml
- name: Run E2E tests
  run: |
    docker compose up -d service-one service-two ui
    docker compose run --rm playwright-tests
    docker compose down
```

❌ **Not this:**
```yaml
- name: Start service
  run: dotnet run &  # Background processes don't work reliably
```

### 2. Wait for Health Checks

```bash
# Wait until all services are healthy
timeout 120 bash -c 'until docker compose ps | grep -q "healthy.*healthy.*healthy"; do
  sleep 5
done'
```

### 3. Show Logs on Failure

```yaml
- name: Show service logs on failure
  if: failure()
  run: |
    docker compose logs service-one
    docker compose logs service-two
    docker compose logs ui
```

### 4. Clean Up Always

```yaml
- name: Stop services
  if: always()
  run: docker compose down -v  # -v removes volumes too
```

## 💡 Tips

### Speed Up Builds

Use Docker layer caching:
```yaml
- name: Set up Docker Buildx
  uses: docker/setup-buildx-action@v3
  with:
    cache-from: type=gha  # GitHub Actions cache
    cache-to: type=gha,mode=max
```

### Local Development Workflow

```bash
# 1. Start services
docker compose up -d

# 2. Develop and test locally (hot reload works)
cd src/Service-One
dotnet watch run

# 3. Run tests when ready
dotnet test tests/Playwright.Tests/

# 4. Stop services
docker compose down
```

### Debugging Tests

```bash
# Run tests with headed browser
docker compose run --rm -e PlaywrightSettings__Headless=false playwright-tests

# Or run tests locally and attach debugger
dotnet test tests/Playwright.Tests/ --filter "TestName"
```

## 📚 Additional Resources

- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [Playwright Docker Guide](https://playwright.dev/dotnet/docs/ci)
- [GitHub Actions with Docker](https://docs.github.com/en/actions/use-cases-and-examples/publishing-packages/publishing-docker-images)

## 🎯 Summary

✅ **Benefits of Docker approach:**
- Works identically locally and in CI
- Services isolated in containers
- Health checks ensure services are ready
- Easy to debug with logs
- Scales to more services easily

✅ **When to use:**
- CI/CD pipelines (always)
- Local E2E testing
- Integration testing
- Demonstrating full system

✅ **When NOT to use:**
- Unit testing (too slow)
- Quick iterative development (use `dotnet watch`)
- Debugging individual services
