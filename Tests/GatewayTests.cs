using System.Collections.Generic;
using Xunit;

namespace Tests;

public class GatewayTests
{
    [Fact]
    public void Gateways_CanConnect()
    {
        IGateway here = new OfflineGateway("Alice");
        IGateway there = new OfflineGateway("Bob");

        var invite = there.GetInvitation();
        here.TryInvitation(invite);

        Assert.Contains("Bob", here.ConnectedPeers);
        Assert.Contains("Alice", there.ConnectedPeers);
    }
}

public class OfflineGateway : IGateway
{
    private static readonly Dictionary<string, OfflineGateway> GlobalOwnerToGatewayMap = new();

    private readonly string _owner;
    private readonly List<string> _peers = new();

    public OfflineGateway(string owner)
    {
        _owner = owner;
        GlobalOwnerToGatewayMap.Add(owner, this);
    }

    public IEnumerable<string> ConnectedPeers => _peers;

    public string GetInvitation()
    {
        return _owner;
    }

    public void TryInvitation(string peerOwner)
    {
        AddPeer(peerOwner);
        var peerGateway = GlobalOwnerToGatewayMap[peerOwner];
        peerGateway.AddPeer(_owner);
    }

    private void AddPeer(string peerOwner)
    {
        _peers.Add(peerOwner);
    }
}

public interface IGateway
{
    IEnumerable<string> ConnectedPeers { get; }
    string GetInvitation();
    void TryInvitation(string invite);
}