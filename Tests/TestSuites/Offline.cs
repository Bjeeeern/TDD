using System;
using System.Linq;
using Core;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites;

public class Offline
{
    private readonly Conversation _conversation;
    private readonly Message _greeting;
    private readonly Message _question;

    public Offline()
    {
        _conversation = new Conversation();
        _greeting = new Message
        {
            Timestamp = DateTime.Now,
            Content = "Hi!"
        };
        _question = new Message
        {
            Timestamp = _greeting.Timestamp + TimeSpan.FromMinutes(1),
            Content = "What's up?"
        };
    }

    [Fact]
    public void ChatClient_CanReceiveTwoMessages()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_question);

        var messages = _conversation.PollMessages(2);

        AssertX.Length(2, messages);
        Assert.Contains(_greeting, messages);
        Assert.Contains(_question, messages);
    }

    [Fact]
    public void Messages_ArePolledInChronologicalOrder()
    {
        _conversation.ReceiveMessage(_question);
        _conversation.ReceiveMessage(_greeting);

        var messages = _conversation.PollMessages(2);

        Assert.Equal(_greeting, messages.First());
        Assert.Equal(_question, messages.Skip(1).First());
    }

    [Fact]
    public void ChatClient_CanPollLessThanReceived()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_question);

        var messages = _conversation.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(_greeting, messages);
    }

    [Fact]
    public void ChatClient_CantPollMoreThanReceived()
    {
        _conversation.ReceiveMessage(_greeting);
        _conversation.ReceiveMessage(_question);

        var messages = _conversation.PollMessages(3);

        AssertX.Length(2, messages);
        Assert.Contains(_greeting, messages);
        Assert.Contains(_question, messages);
    }
}