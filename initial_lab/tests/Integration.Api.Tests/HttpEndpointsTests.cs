using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Api.Tests;

public class HttpEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HttpEndpointsTests(WebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Health_ReturnsHealthy()
    {
        var result = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var json = await result.Content.ReadFromJsonAsync<HealthResponse>();
		Assert.Equal("healthy", json?.Status);
    }

    [Fact]
    public async Task KnownArticle_ReturnsOk()
    {
        var result = await _client.GetAsync("/api/articles/42");
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        var json = await result.Content.ReadFromJsonAsync<ArticleResponse>();
        Assert.Equal(42, json?.Id);
        Assert.Equal(7.90m, json?.PriceEur);
    }

    [Fact]
    public async Task UnknownArticle_Returns404()
    {
        var result = await _client.GetAsync("/api/articles/999");
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    private sealed record HealthResponse(string Status);
    private sealed record ArticleResponse(int Id, string Name, decimal PriceEur);
}
