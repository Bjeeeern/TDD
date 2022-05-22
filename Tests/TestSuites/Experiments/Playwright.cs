using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Playwright;
using Server;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites.Experiments;

public class Playwright : IAsyncLifetime
{
    private IBrowser _browser = null!;
    private IPlaywright _playwright = null!;
    private WebApp _server = null!;

    public async Task InitializeAsync()
    {
        _server = await WebApp.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = CommonPaths.ServerProject(),
            WebRootPath = CommonPaths.ServerWebRoot()
        });

        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
    }

    public async Task DisposeAsync()
    {
        await _server.DisposeAsync();
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }

    [Fact]
    public async Task CanLaunchTheChromiumBrowserAndCreateAContextAndAPage()
    {
        await using var context = await _browser.NewContextAsync();
        await context.NewPageAsync();
    }

    [Fact]
    public async Task CanVisitTheServerIndexPage()
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        var result = await page.GotoAsync("https://localhost:5001");
        Assert.NotNull(result);
        Assert.True(result.Ok);
    }

    [Fact]
    public async Task CanFindAParagraphOnTheIndexPage()
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:5001");

        Assert.True(await page.IsVisibleAsync("p"));
        Assert.Equal("Hello World!", await page.InnerTextAsync("p"));
    }
}