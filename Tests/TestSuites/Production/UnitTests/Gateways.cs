using Core;
using Tests.Utilities.Mocks;
using Xunit;

namespace Tests.TestSuites.Production.UnitTests;

public class Gateways
{
    [Fact]
    public void CanConnect()
    {
        IGateway here = new OfflineGateway("Alice");
        IGateway there = new OfflineGateway("Bob");

        var invite = there.GetInvitation();
        here.TryInvitation(invite);

        Assert.Contains("Bob", here.ConnectedPeers);
        Assert.Contains("Alice", there.ConnectedPeers);
    }
}