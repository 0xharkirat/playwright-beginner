#!/bin/bash
# Build all services locally (outside Docker)

set -e

echo "🔨 Building Services Locally (APIs only)"
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

echo "============================="
echo "✅ All API services built successfully! (UI is built inside Docker)"
echo ""
