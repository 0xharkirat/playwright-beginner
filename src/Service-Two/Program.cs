var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Service-Two";
    config.Title = "Service-Two v1";
    config.Version = "v1";
});

var app = builder.Build();

app.UseOpenApi();
app.UseSwaggerUi();

app.MapGet("/api/World", () => "Hark");

app.Run();
