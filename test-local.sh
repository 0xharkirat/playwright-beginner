#!/bin/bash
# Local Test Runner - Verifies everything works before pushing to GitHub

set -e  # Exit on error

echo "🎭 Playwright Beginner - Local Test Runner"
echo "=========================================="
echo ""

# Check Docker is running
echo "✓ Checking Docker..."
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker Desktop."
    exit 1
fi
echo "✅ Docker is running"
echo ""

# Build images
echo "🔨 Building Docker images..."
docker compose build
echo "✅ Images built successfully"
echo ""

# Start services
echo "🚀 Starting services..."
docker compose up -d service-one service-two ui
echo "✅ Services started"
echo ""

# Wait for health checks
echo "⏳ Waiting for services to be healthy..."
timeout 120 bash -c '
MAX_ATTEMPTS=24
ATTEMPT=0
while [ $ATTEMPT -lt $MAX_ATTEMPTS ]; do
    ATTEMPT=$((ATTEMPT + 1))
    
    # Check each service health status
    SERVICE_ONE=$(docker inspect service-one --format="{{.State.Health.Status}}" 2>/dev/null || echo "starting")
    SERVICE_TWO=$(docker inspect service-two --format="{{.State.Health.Status}}" 2>/dev/null || echo "starting")
    UI=$(docker inspect ui --format="{{.State.Health.Status}}" 2>/dev/null || echo "starting")
    
    echo "   Attempt $ATTEMPT: service-one=$SERVICE_ONE, service-two=$SERVICE_TWO, ui=$UI"
    
    if [ "$SERVICE_ONE" = "healthy" ] && [ "$SERVICE_TWO" = "healthy" ] && [ "$UI" = "healthy" ]; then
        echo "   All services healthy!"
        break
    fi
    
    sleep 5
done

if [ $ATTEMPT -ge $MAX_ATTEMPTS ]; then
    exit 1
fi
' || {
    echo "❌ Services failed to become healthy"
    echo "📋 Service logs:"
    docker compose logs
    docker compose down
    exit 1
}
echo "✅ All services are healthy"
echo ""

# Show service status
echo "📊 Service Status:"
docker compose ps
echo ""

# Run tests
echo "🎭 Running Playwright tests..."
if docker compose run --rm playwright-tests; then
    echo ""
    echo "✅ All tests passed!"
    TEST_PASSED=true
else
    echo ""
    echo "❌ Some tests failed"
    TEST_PASSED=false
fi
echo ""

# Show test results
if [ -d "tests/Playwright.Tests/TestResults" ]; then
    echo "📊 Test Results:"
    find tests/Playwright.Tests/TestResults -name "*.trx" -exec echo "   {}" \;
    echo ""
fi

# Show coverage if available
if [ -d "coverage-report" ]; then
    echo "📈 Coverage Report:"
    echo "   coverage-report/index.html"
    echo ""
fi

# Cleanup
echo "🧹 Cleaning up..."
docker compose down
echo "✅ Services stopped"
echo ""

# Final summary
echo "=========================================="
if [ "$TEST_PASSED" = true ]; then
    echo "✅ SUCCESS! Everything works locally."
    echo ""
    echo "Next steps:"
    echo "1. Review test results in tests/Playwright.Tests/TestResults/"
    echo "2. Check coverage report: open coverage-report/index.html"
    echo "3. Ready to push to GitHub!"
    echo ""
    echo "   git add ."
    echo "   git commit -m \"feat: Add Docker-based CI/CD\""
    echo "   git push origin main"
    exit 0
else
    echo "❌ FAILED! Fix test failures before pushing."
    echo ""
    echo "Troubleshooting:"
    echo "1. Check service logs: docker compose logs service-one"
    echo "2. Verify services manually:"
    echo "   - http://localhost:5001/swagger"
    echo "   - http://localhost:5002/swagger"
    echo "   - http://localhost:3000"
    echo "3. See _docs/LOCAL_TESTING.md for help"
    exit 1
fi
