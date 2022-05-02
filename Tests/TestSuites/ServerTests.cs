using System.Threading.Tasks;
using Microsoft.Playwright;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites;

public class ServerTests : AssertExtentions, IClassFixture<HttpServerFixture>
{
    private readonly HttpServerFixture _server;

    public ServerTests(HttpServerFixture server)
    {
        _server = server;
    }

    [Fact]
    public async Task Server_HasIndexPage()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync(new BrowserNewPageOptions {IgnoreHTTPSErrors = true});

        var result = await page.GotoAsync(_server.ServerAddress);

        True(result.Ok);
    }
}