using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class SaludoTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public SaludoTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Saludo_DevuelveHolaMundo()
    {
        var response = await _client.GetAsync("/api/saludo");
        var contenido = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Hola mundo", contenido);
    }
}
