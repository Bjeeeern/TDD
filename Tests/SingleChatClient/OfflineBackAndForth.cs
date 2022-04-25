using System;
using System.Linq;
using Tests.Utilities;
using Xunit;

namespace Tests.SingleChatClient;

public class OfflineBackAndForth
{
    private readonly Conversation _conversation;
    private readonly Message _greeting;
    private readonly Message _response;

    public OfflineBackAndForth()
    {
        _conversation = new Conversation();
        _greeting = new Message {Timestamp = DateTime.Now, Content = "Hi!"};
        _response = new Message
            {Timestamp = _greeting.Timestamp + TimeSpan.FromMinutes(1), Content = "What's up?"};
    }

    [Fact]
    public void ChatClient_CanReceiveTwoMessages()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(2);

        AssertX.Length(2, messages);
        Assert.Contains(_greeting, messages);
        Assert.Contains(_response, messages);
    }

    [Fact]
    public void Messages_ArePolledInChronologicalOrder()
    {
        _conversation.ReceiveMessage(_response);
        _conversation.ReceiveMessage(_greeting);

        var messages = _conversation.PollMessages(2);

        Assert.Equal(_greeting, messages.First());
        Assert.Equal(_response, messages.Skip(1).First());
    }

    [Fact]
    public void ChatClient_CanPollLessThanReceived()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(_greeting, messages);
    }

    [Fact]
    public void ChatClient_CantPollMoreThanReceived()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(3);

        AssertX.Length(2, messages);
        Assert.Contains(_greeting, messages);
        Assert.Contains(_response, messages);
    }
}