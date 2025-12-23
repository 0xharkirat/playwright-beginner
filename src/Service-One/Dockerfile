# Service-One Runtime Dockerfile
# Expects pre-built artifacts in bin/Release/net9.0/publish/
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy pre-built publish output
COPY src/Service-One/bin/Release/net9.0/publish/ .

EXPOSE 5001
ENV ASPNETCORE_URLS=http://+:5001

ENTRYPOINT ["dotnet", "Service-One.dll"]
