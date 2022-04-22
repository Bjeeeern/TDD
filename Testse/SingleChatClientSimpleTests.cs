using System;
using Xunit;

namespace Tests;

public class SingleChatClientSimpleTests
{
    private readonly ChatClient _client;

    public SingleChatClientSimpleTests()
    {
        _client = new ChatClient();
    }

    [Fact]
    public void NewlyCreatedChatClient_HasNoMessages()
    {
        var messages = _client.PollMessages(1);

        Assert.Empty(messages);
    }

    [Fact]
    public void ChatClient_CanReceiveAMessage()
    {
        var testGreeting = new Message {Timestamp = DateTime.Now, Content = "こんにちは"};

        _client.ReceiveMessage(testGreeting);

        var messages = _client.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(testGreeting, messages);
    }
}