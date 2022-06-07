using Microsoft.Playwright;
using Tests.Utilities.Fixtures;
using Xunit;

namespace Tests.TestSuites.Production.IntergrationTests;

public class TwoClientsChatting : IClassFixture<ServerAndTwoClientsFixture>
{
    private readonly string _certifiedServerUri;
    private readonly IPage _clientA;
    private readonly IPage _clientB;

    public TwoClientsChatting(ServerAndTwoClientsFixture fixture)
    {
        _clientA = fixture.ClientA;
        _clientB = fixture.ClientB;
        _certifiedServerUri = fixture.CertifiedServerUri;
    }
}