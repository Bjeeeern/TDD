using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Playwright;
using Server;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites;

public class ServerTests : AssertExtentions
{
    [Fact]
    public async Task Server_HasIndexPage()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync(new BrowserNewPageOptions
        {
            IgnoreHTTPSErrors = true
        });

        await using var server = await ServerFactory.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            ContentRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\",
            WebRootPath = @"C:\Users\bjeee\RiderProjects\TDD\Server\wwwroot\"
        });

        var result = await page.GotoAsync("https://localhost:5001");
        True(result.Ok);
    }
}