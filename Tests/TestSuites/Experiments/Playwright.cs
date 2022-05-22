using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Server;
using Xunit;

namespace Tests.TestSuites.Experiments;

public class Playwright
{
    [Fact]
    public async Task Server_CanStart()
    {
        await using var server = await WebApp.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\",
            WebRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\wwwroot\"
        });
    }

    [Fact]
    public async Task Playwright_CanLaunchChromiumBrowserAndContextAndPage()
    {
        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        await context.NewPageAsync();
    }

    [Fact]
    public async Task Server_ReturnsIndexPage()
    {
        await using var server = await WebApp.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\",
            WebRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\wwwroot\"
        });

        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        var result = await page.GotoAsync("https://localhost:5001");
        Assert.NotNull(result);
        Assert.True(result.Ok);
    }

    [Fact]
    public async Task IndexPage_HasElement()
    {
        await using var server = await WebApp.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\",
            WebRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\wwwroot\"
        });

        using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        await page.GotoAsync("https://localhost:5001");

        Assert.True(await page.IsVisibleAsync("p"));
        Assert.Equal("Hello World!", await page.InnerTextAsync("p"));
    }
}