using System.Threading.Tasks;
using Microsoft.Playwright;
using Tests.Utilities.Fixtures;
using Xunit;
using Assert = Tests.Utilities.AssertExtentions;

namespace Tests.TestSuites.Production.IntergrationTests;

public class SingleClientChatting : IClassFixture<ServerAndClientFixture>
{
    private readonly string _certifiedServerUri;
    private readonly IPage _page;

    public SingleClientChatting(ServerAndClientFixture fixture)
    {
        _page = fixture.Page;
        _certifiedServerUri = fixture.CertifiedServerUri;
    }

    [Fact]
    public async Task NoMessagesAtStart()
    {
        await _page.GotoAsync(_certifiedServerUri);
        await _page.WaitForSelectorAsync("#message-input-area");

        var messageElement = await _page.QuerySelectorAsync("#chat-history .message");

        Assert.Null(messageElement);
    }

    [Fact]
    public async Task CanEnterMessage()
    {
        await _page.GotoAsync(_certifiedServerUri);
        await _page.WaitForSelectorAsync("#message-input-area");

        var testMessage = "test";
        await _page.TypeAsync("#message-input-area #input-field", testMessage);
        await _page.ClickAsync("#message-input-area #send-message-btn");

        var messageElement = await _page.QuerySelectorAsync("#chat-history .message");

        Assert.True(await messageElement.IsVisibleAsync(), ".message element not visible");
        Assert.Equal(testMessage, await messageElement.InnerTextAsync());
    }
}