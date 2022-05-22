using System.Collections.Generic;
using Core;

namespace Tests.Utilities;

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