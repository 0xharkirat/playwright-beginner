# 🚀 Local Testing Guide

This guide shows you how to test everything locally with Docker before pushing to GitHub.

## Prerequisites

- [Docker Desktop](https://docs.docker.com/get-docker/) installed and running
- Project cloned locally

## 🎯 Step-by-Step Local Testing

### 1. Build Docker Images

```bash
cd /path/to/playwright-beginner

# Build all images
docker compose build
```

**Expected output:**
- Service-One image built
- Service-Two image built
- UI image built
- Playwright-tests image built

### 2. Start Services

```bash
# Start all services in background
docker compose up -d service-one service-two ui
```

### 3. Verify Services are Running

```bash
# Check service status
docker compose ps
```

**You should see:**
```
NAME          STATUS        PORTS
service-one   Up (healthy)  0.0.0.0:5001->5001/tcp
service-two   Up (healthy)  0.0.0.0:5002->5002/tcp
ui            Up (healthy)  0.0.0.0:3000->3000/tcp
```

**Test in browser:**
- Service-One Swagger: http://localhost:5001/swagger
- Service-Two Swagger: http://localhost:5002/swagger
- UI: http://localhost:3000

### 4. Run Playwright Tests

```bash
# Run tests in Docker container
docker compose run --rm playwright-tests
```

**Expected output:**
```
Starting test execution, please wait...
✅ Test HelloPage_ShouldDisplay_HelloMessage_InParagraph passed
✅ Test HelloPage_POM_ShouldDisplay_HelloMessage passed
✅ Test HelloPage_POM_ShouldDisplay_Heading passed
✅ Test HelloPage_POM_ShouldHave_ParagraphVisible passed

Test Run Successful.
Total tests: 4
     Passed: 4
```

### 5. View Test Results

```bash
# Check test results directory
ls -la tests/Playwright.Tests/TestResults/

# View coverage report
open coverage-report/index.html  # macOS
# or
xdg-open coverage-report/index.html  # Linux
```

### 6. Clean Up

```bash
# Stop all services
docker compose down

# Remove volumes (optional - cleans everything)
docker compose down -v
```

## 🐛 Troubleshooting Local Testing

### Issue: Services not starting

**Check logs:**
```bash
docker compose logs service-one
docker compose logs service-two
docker compose logs ui
```

**Common fixes:**
```bash
# Rebuild without cache
docker compose build --no-cache

# Check if ports are in use
lsof -i :5001
lsof -i :5002
lsof -i :3000
```

### Issue: Tests can't reach services

**Verify network:**
```bash
docker network ls
docker network inspect playwright-beginner_app-network
```

**Check container names:**
```bash
docker compose ps
```

### Issue: Health checks failing

**Increase timeout in docker-compose.yml:**
```yaml
healthcheck:
  start_period: 30s  # Increase from 10s
  interval: 15s
  retries: 10
```

### Issue: Build failures

**Check Dockerfile syntax:**
```bash
# Build individual service
docker compose build service-one

# Check build logs
docker compose build service-one --progress=plain
```

## 💡 Development Workflow

### Quick Iteration Loop

```bash
# 1. Start services once
docker compose up -d

# 2. Make code changes to your service

# 3. Rebuild and restart just that service
docker compose up -d --build service-one

# 4. Run tests
docker compose run --rm playwright-tests

# 5. Repeat steps 2-4
```

### Debug Mode

```bash
# Run services with logs visible
docker compose up service-one service-two ui

# In another terminal, run tests
docker compose run --rm playwright-tests
```

### Run Tests with Headed Browser

```bash
# Not possible in Docker (no display)
# Instead, run tests locally:
cd tests/Playwright.Tests
dotnet test
```

## 📊 Verifying Everything Works

### Checklist Before Pushing to GitHub

- [ ] All Docker images build successfully
- [ ] All services start and become healthy
- [ ] Service-One Swagger loads in browser
- [ ] Service-Two Swagger loads in browser
- [ ] UI loads in browser
- [ ] All Playwright tests pass
- [ ] Coverage report generated
- [ ] No errors in service logs

### Test the Exact CI Flow Locally

```bash
# This mimics what GitHub Actions will do
docker compose build
docker compose up -d service-one service-two ui
docker compose run --rm playwright-tests
docker compose down -v
```

If this works locally, it will work in GitHub Actions! ✅

## 🎯 Common Commands Reference

```bash
# Build everything
docker compose build

# Start services
docker compose up -d

# Check status
docker compose ps

# View logs
docker compose logs -f service-one

# Run tests
docker compose run --rm playwright-tests

# Stop services
docker compose down

# Clean everything
docker compose down -v
docker system prune -a

# Rebuild single service
docker compose build service-one
docker compose up -d --build service-one
```

## 🚀 When Everything Works Locally...

### Push to GitHub

```bash
git add .
git commit -m "feat: Add Docker-based CI/CD"
git push origin main
```

### GitHub Actions Will:
1. Clone your repo
2. Build all Docker images (cached layers speed this up)
3. Start services with `docker compose up -d`
4. Wait for health checks
5. Run Playwright tests
6. Generate coverage reports
7. Upload artifacts
8. Clean up

**Same commands, same results!** 🎉

## 💡 Pro Tips

### Speed Up Local Builds

```bash
# Use BuildKit for faster builds
export DOCKER_BUILDKIT=1
export COMPOSE_DOCKER_CLI_BUILD=1

docker compose build
```

### Keep Images Updated

```bash
# Pull latest base images periodically
docker compose pull
docker compose build --pull
```

### Monitor Resource Usage

```bash
# Check Docker resource usage
docker stats

# Check disk space
docker system df
```

### Debugging Container Issues

```bash
# Enter a running container
docker compose exec service-one bash

# Run commands inside container
docker compose exec service-one ls -la
docker compose exec service-one curl http://localhost:5001/swagger
```

## ✅ Success Criteria

Your local setup is ready when:

✅ `docker compose build` completes without errors  
✅ `docker compose up -d` starts all services  
✅ `docker compose ps` shows all services as "healthy"  
✅ Services are accessible in browser  
✅ `docker compose run --rm playwright-tests` passes all tests  
✅ Coverage report is generated  
✅ `docker compose down` stops everything cleanly  

**Once all these pass, you're ready for CI/CD!** 🚀
