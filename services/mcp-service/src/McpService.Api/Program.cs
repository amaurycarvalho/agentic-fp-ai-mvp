using McpService.Api.Application;
using McpService.Api.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBasicCountService, BasicCountService>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new
{
    service = "mcp-service",
    status = "healthy",
    timestampUtc = DateTime.UtcNow
}));

app.MapPost("/count/basic", (CountBasicRequest request, IBasicCountService basicCountService) =>
{
    if (string.IsNullOrWhiteSpace(request.UserStory))
    {
        return Results.BadRequest(new { error = "The field 'userStory' is required." });
    }

    var result = basicCountService.Analyze(request);
    return Results.Ok(result);
});

app.Run();
