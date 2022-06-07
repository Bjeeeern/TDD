using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Playwright;
using Server;
using Xunit;

namespace Tests.Utilities.Fixtures;

public class ServerAndTwoClientsFixture : IAsyncLifetime
{
    public IPage ClientA = null!;
    public IPage ClientB = null!;
    public string CertifiedServerUri = null!;
    
    private IBrowser _browser = null!;
    private IBrowserContext _contextA = null!;
    private IBrowserContext _contextB = null!;
    private IPlaywright _playwright = null!;
    private WebApplicationRunner Server = null!;


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

        _contextA = await _browser.NewContextAsync();
        _contextA.SetDefaultTimeout(Global.PlaywrightTimeout);
        ClientA = await _contextA.NewPageAsync();

        _contextB = await _browser.NewContextAsync();
        _contextB.SetDefaultTimeout(Global.PlaywrightTimeout);
        ClientB = await _contextB.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        await Server.DisposeAsync();
        await _contextA.DisposeAsync();
        await _contextB.DisposeAsync();
        await _browser.DisposeAsync();
        _playwright.Dispose();
    }
}