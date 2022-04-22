using System;
using System.Linq;
using Xunit;

namespace Tests;

public class SingleChatClientBackAndForth
{
    private readonly ChatClient _client;
    private readonly Message _testGreeting;
    private readonly Message _testResponse;

    public SingleChatClientBackAndForth()
    {
        _client = new ChatClient();
        _testGreeting = new Message {Timestamp = DateTime.Now, Content = "こんにちは"};
        _testResponse = new Message
            {Timestamp = _testGreeting.Timestamp + TimeSpan.FromMinutes(1), Content = "こんにちは"};
    }

    [Fact]
    public void ChatClient_CanReceiveTwoMessages()
    {
        _client.ReceiveMessage(_testGreeting);
        _client.ReceiveMessage(_testResponse);

        var messages = _client.PollMessages(2);

        AssertX.Length(2, messages);
        Assert.Contains(_testGreeting, messages);
        Assert.Contains(_testResponse, messages);
    }

    [Fact]
    public void Messages_ArePolledInChronologicalOrder()
    {
        _client.ReceiveMessage(_testResponse);
        _client.ReceiveMessage(_testGreeting);

        var messages = _client.PollMessages(2);

        Assert.Equal(_testGreeting, messages.First());
        Assert.Equal(_testResponse, messages.Skip(1).First());
    }

    [Fact]
    public void ChatClient_CanPollLessThanReceived()
    {
        _client.ReceiveMessage(_testGreeting);
        _client.ReceiveMessage(_testResponse);

        var messages = _client.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(_testGreeting, messages);
    }

    [Fact]
    public void ChatClient_CantPollMoreThanReceived()
    {
        _client.ReceiveMessage(_testGreeting);
        _client.ReceiveMessage(_testResponse);

        var messages = _client.PollMessages(3);

        AssertX.Length(2, messages);
        Assert.Contains(_testGreeting, messages);
        Assert.Contains(_testResponse, messages);
    }
}