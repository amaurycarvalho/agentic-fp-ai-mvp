var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new
{
    service = "mcp-service",
    status = "healthy",
    timestampUtc = DateTime.UtcNow
}));

app.Run();
