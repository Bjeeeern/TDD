using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Playwright;
using Server;
using Xunit;

namespace Tests.Utilities.Fixtures;

public class ServerAndClientFixture : IAsyncLifetime
{
    public string CertifiedServerUri = null!;
    public IPage Page = null!;
    
    private WebApplicationRunner Server = null!;
    private IBrowser _browser = null!;
    private IBrowserContext _context = null!;
    private IPlaywright _playwright = null!;


    public async Task InitializeAsync()
    {
        Server = await WebApplicationRunner.RunWithOptions(new WebApplicationOptions
        {
            ApplicationName = "Server",
            EnvironmentName = "Development",
            Args = new[] {"--urls", "https://127.0.0.1:0;http://127.0.0.1:0"}
        });
        CertifiedServerUri = Server.HttpsUri.Replace("127.0.0.1", "localhost");

        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync();
        _context = await _browser.NewContextAsync();
        _context.SetDefaultTimeout(Global.PlaywrightTimeout);
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