using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using McpService.Api.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;

namespace McpService.Api.Tests;

public class McpServiceApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public McpServiceApiTests(WebApplicationFactory<Program> factory)
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

        Assert.Equal("mcp-service", root.GetProperty("service").GetString());
        Assert.Equal("healthy", root.GetProperty("status").GetString());
        Assert.True(root.TryGetProperty("timestampUtc", out var timestamp));
        Assert.True(timestamp.TryGetDateTime(out _));
    }

    [Fact]
    public async Task CountBasic_ReturnsCount_ForValidUserStory()
    {
        var payload = new { userStory = "Como analista, preciso cadastrar clientes e armazenar dados no banco." };

        var response = await _client.PostAsJsonAsync("/count/basic", payload, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CountBasicResponse>(TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotEmpty(result!.TransactionalFunctions);
        Assert.Contains(result.TransactionalFunctions, x => x.Type == "EI");
        Assert.Contains(result.DataFunctions, x => x.Type == "ILF");
        Assert.NotEmpty(result.AuditTrail);
        Assert.True(result.Summary.TotalFunctionPoints > 0);
    }

    [Fact]
    public async Task CountBasic_ReturnsBadRequest_WhenUserStoryMissing()
    {
        var payload = new { det = 10 };

        var response = await _client.PostAsJsonAsync("/count/basic", payload, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        var message = json.RootElement.GetProperty("error").GetString();
        Assert.Contains("userStory", message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UnknownRoute_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/not-a-route", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
