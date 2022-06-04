using System.Threading.Tasks;
using Microsoft.Playwright;
using Server;
using Tests.Utilities.Fixtures;
using Xunit;

namespace Tests.TestSuites.Experiments;

public class Playwright : IClassFixture<ServerClientFixture>
{
    private readonly IPage _page;
    private readonly WebApplicationRunner _server;

    public Playwright(ServerClientFixture fixture)
    {
        _page = fixture.Page;
        _server = fixture.Server;
    }

    private string CertifiedServerUri => _server.HttpsUri.Replace("127.0.0.1", "localhost");

    [Fact]
    public async Task CanVisitTheServerIndexPage()
    {
        var result = await _page.GotoAsync(CertifiedServerUri);

        Assert.Equal(200, result.Status);
    }

    [Fact]
    public async Task CanFindAParagraphOnTheIndexPage()
    {
        await _page.GotoAsync(CertifiedServerUri);
        await _page.WaitForSelectorAsync("main");

        Assert.True(await _page.IsVisibleAsync("p"));
        Assert.Equal("Hello World!", await _page.InnerTextAsync("p"));
    }

    [Fact]
    public async Task CanNavigateToNonIndexPageAndRefresh()
    {
        await _page.GotoAsync(CertifiedServerUri);
        await _page.ClickAsync("button#about");

        Assert.Contains("/about", _page.Url.ToLower());

        var result = await _page.ReloadAsync();

        Assert.Equal(200, result.Status);
    }
}