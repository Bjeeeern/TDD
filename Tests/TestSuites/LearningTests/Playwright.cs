using System.Threading.Tasks;
using Microsoft.Playwright;
using Tests.Utilities.Fixtures;
using Xunit;

namespace Tests.TestSuites.LearningTests;

public class Playwright : IClassFixture<ServerAndClientFixture>
{
    private readonly string _certifiedServerUri;
    private readonly IPage _page;

    public Playwright(ServerAndClientFixture fixture)
    {
        _page = fixture.Page;
        _certifiedServerUri = fixture.CertifiedServerUri;
    }

    [Fact]
    public async Task CanVisitTheServerIndexPage()
    {
        var result = await _page.GotoAsync(_certifiedServerUri);

        Assert.Equal(200, result.Status);
    }

    [Fact]
    public async Task CanFindAParagraphOnTheIndexPage()
    {
        await _page.GotoAsync(_certifiedServerUri);
        await _page.WaitForSelectorAsync("main");

        Assert.True(await _page.IsVisibleAsync("p"));
        Assert.Equal("Hello World!", await _page.InnerTextAsync("p"));
    }

    [Fact]
    public async Task CanFollowAnAboutPageLinkFromTheIndexPage()
    {
        await _page.GotoAsync(_certifiedServerUri);
        await _page.ClickAsync("button#about");

        Assert.Contains("/about", _page.Url.ToLower());
    }

    [Fact]
    public async Task CanRefreshTheAboutPage()
    {
        await _page.GotoAsync(_certifiedServerUri + "/about");

        var result = await _page.ReloadAsync();

        Assert.Equal(200, result.Status);
    }
}