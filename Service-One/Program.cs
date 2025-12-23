var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJS", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure HttpClient for Service-Two
var serviceTwoUrl = builder.Configuration["ServiceTwo:BaseUrl"] ?? "http://localhost:5049";
builder.Services.AddHttpClient("ServiceTwo", client =>
{
    client.BaseAddress = new Uri(serviceTwoUrl);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Service-One";
    config.Title = "Service-One v1";
    config.Version = "v1";
});

var app = builder.Build();

app.UseCors("AllowNextJS");

app.UseOpenApi();
app.UseSwaggerUi();


app.MapGet("/api/Hello", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient("ServiceTwo");
    var world = await client.GetStringAsync("/api/World");
    return $"Hello {world}";
});

app.Run();
