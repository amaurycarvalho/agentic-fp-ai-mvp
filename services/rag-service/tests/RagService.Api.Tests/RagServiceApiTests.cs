using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RagService.Api.Tests;

public class RagServiceApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RagServiceApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ReturnsOk_WithExpectedPayload()
    {
        var response = await _client.GetAsync("/health", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        var root = json.RootElement;

        Assert.Equal("rag-service", root.GetProperty("service").GetString());
        Assert.Equal("healthy", root.GetProperty("status").GetString());
        Assert.True(root.TryGetProperty("timestampUtc", out var timestamp));
        Assert.True(timestamp.TryGetDateTime(out _));
    }

    [Fact]
    public async Task UnknownRoute_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/not-a-route", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
