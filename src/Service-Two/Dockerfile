# Service-Two Runtime Dockerfile
# Expects pre-built artifacts in bin/Release/net9.0/publish/
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy pre-built publish output
COPY src/Service-Two/bin/Release/net9.0/publish/ .

EXPOSE 5002
ENV ASPNETCORE_URLS=http://+:5002

ENTRYPOINT ["dotnet", "Service-Two.dll"]
