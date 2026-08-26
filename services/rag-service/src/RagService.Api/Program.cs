var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new
{
    service = "rag-service",
    status = "healthy",
    timestampUtc = DateTime.UtcNow
}));

app.Run();

public partial class Program { }
