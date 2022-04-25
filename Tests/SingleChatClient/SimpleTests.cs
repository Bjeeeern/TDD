using System;
using Xunit;

namespace Tests.SingleChatClient;

public class SimpleTests
{
    private readonly Conversation _conversation;

    public SimpleTests()
    {
        _conversation = new Conversation();
    }

    [Fact]
    public void NewlyCreatedChatClient_HasNoMessages()
    {
        var messages = _conversation.PollMessages(1);

        Assert.Empty(messages);
    }

    [Fact]
    public void ZeroPolling_HasNoMessages()
    {
        var messages = _conversation.PollMessages(0);

        Assert.Empty(messages);
    }

    [Fact]
    public void NegativePolling_HasNoMessages()
    {
        var messages = _conversation.PollMessages(-1);

        Assert.Empty(messages);
    }

    [Fact]
    public void ChatClient_CanReceiveAMessage()
    {
        var greeting = new Message {Timestamp = DateTime.Now, Content = "Hi!"};

        _conversation.ReceiveMessage(greeting);

        var messages = _conversation.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(greeting, messages);
    }
}