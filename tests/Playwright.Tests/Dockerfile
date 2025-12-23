# Playwright Tests Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

# Install PowerShell for Playwright installation
RUN apt-get update && apt-get install -y \
    wget \
    && rm -rf /var/lib/apt/lists/*

# Copy test project files
COPY Playwright.Tests.csproj .
RUN dotnet restore

# Copy everything else
COPY . .

# Build the test project
RUN dotnet build -c Release

# Install Playwright browsers and dependencies
RUN pwsh bin/Release/net9.0/playwright.ps1 install --with-deps chromium

# Install ReportGenerator for coverage
RUN dotnet tool install -g dotnet-reportgenerator-globaltool
ENV PATH="${PATH}:/root/.dotnet/tools"

# Run tests with coverage
CMD ["dotnet", "test", \
     "--no-build", \
     "--configuration", "Release", \
     "--logger", "trx;LogFileName=test-results.trx", \
     "--collect:XPlat Code Coverage", \
     "--results-directory", "./TestResults", \
     "--", "DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover"]
