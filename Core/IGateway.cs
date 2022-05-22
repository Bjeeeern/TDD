namespace Core;

public interface IGateway
{
    IEnumerable<string> ConnectedPeers { get; }
    string GetInvitation();
    void TryInvitation(string invite);
}