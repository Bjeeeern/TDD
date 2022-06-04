using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Playwright;
using Server;
using Xunit;

namespace Tests.Utilities.Fixtures;

public class ServerClientFixture : IAsyncLifetime
{
    private IBrowser _browser = null!;
    private IBrowserContext _context = null!;
    private IPlaywright _playwright = null!;
    public IPage Page = null!;
    public WebApplicationRunner Server = null!;

    public async Task InitializeAsync()
    {
        Server = await WebApplicationRunner.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            Args = new[] {"--urls", "https://127.0.0.1:0;http://127.0.0.1:0"}
        });

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _context = await _browser.NewContextAsync();
        _context.SetDefaultTimeout(2000);
        Page = await _context.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await Server.DisposeAsync();
        await _context.DisposeAsync();
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }
}