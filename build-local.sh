#!/bin/bash
# Build all services locally (outside Docker)

set -e

echo "🔨 Building Services Locally"
echo "============================="
echo ""

# Build Service-One
echo "📦 Building Service-One..."
cd src/Service-One
dotnet publish -c Release
cd ../..
echo "✅ Service-One built"
echo ""

# Build Service-Two
echo "📦 Building Service-Two..."
cd src/Service-Two
dotnet publish -c Release
cd ../..
echo "✅ Service-Two built"
echo ""

# Build UI
echo "📦 Building UI..."
cd ui
if [ ! -d "node_modules" ]; then
    echo "Installing UI dependencies..."
    pnpm install --frozen-lockfile
fi
pnpm run build
cd ..
echo "✅ UI built"
echo ""

echo "============================="
echo "✅ All services built successfully!"
echo ""
