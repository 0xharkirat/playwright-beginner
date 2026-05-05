# Service URLs Configuration

## Overview
Services communicate differently in local development vs Docker environments.

## Configuration

### Local Development (dotnet run)
When running services directly with `dotnet run`:
- Service-One: `http://localhost:5001`
- Service-Two: `http://localhost:5002`
- UI: `http://localhost:3000`

**Configuration files:**
- `src/Service-One/appsettings.json`: `ServiceTwo:BaseUrl = http://localhost:5002`
- `ui/.env.local`: `NEXT_PUBLIC_SERVICE_ONE_URL=http://localhost:5001`

### Docker Environment
Inside Docker network, services use container names:
- Service-One: `http://service-one:5001`
- Service-Two: `http://service-two:5002`
- UI: `http://ui:3000`

**Configuration:**
- Set via environment variables in `docker-compose.yml`
- Service-One: `ServiceTwo__BaseUrl=http://service-two:5002`
- UI: `NEXT_PUBLIC_SERVICE_ONE_URL=http://service-one:5001`

## How It Works

### Service-One → Service-Two
```csharp
// Program.cs reads from configuration
var serviceTwoUrl = builder.Configuration["ServiceTwo:BaseUrl"];
builder.Services.AddHttpClient("ServiceTwo", client =>
{
    client.BaseAddress = new Uri(serviceTwoUrl);
});
```

**Local:** Uses `appsettings.json` → `http://localhost:5002`  
**Docker:** Environment variable overrides → `http://service-two:5002`

### UI → Service-One
```typescript
// page.tsx uses environment variable with fallback
const apiUrl = process.env.NEXT_PUBLIC_SERVICE_ONE_URL || "http://localhost:5001";
const response = await fetch(`${apiUrl}/api/Hello`);
```

**Local:** Uses `.env.local` → `http://localhost:5001`  
**Docker:** Environment variable from docker-compose.yml → `http://service-one:5001`

## Port Mapping

Docker exposes ports to host machine:
```yaml
ports:
  - "5001:5001"  # host:container
```

- **Inside Docker network:** Services use container names (e.g., `http://service-one:5001`)
- **From host machine:** Access via localhost (e.g., `http://localhost:5001`)
- **Playwright tests in Docker:** Use container names since tests run inside network

## CORS Configuration

Service-One allows requests from both environments:
```csharp
policy.WithOrigins("http://localhost:3000", "http://ui:3000")
```

This enables:
- Local UI (`localhost:3000`) → Local API (`localhost:5001`)
- Docker UI (`ui:3000`) → Docker API (`service-one:5001`)

## Environment Variable Priority

.NET Configuration hierarchy (highest to lowest):
1. Environment variables (Docker)
2. appsettings.Development.json
3. appsettings.json

Next.js:
1. Environment variables (Docker)
2. .env.local
3. Hardcoded fallback in code
