using System;
using Core;
using Tests.Utilities;
using Xunit;

namespace Tests.TestSuites;

public class SimplePolling : AssertExtentions
{
    private readonly Conversation _conversation;

    public SimplePolling()
    {
        _conversation = new Conversation();
    }

    [Fact]
    public void NewlyCreatedChatClient_HasNoMessages()
    {
        var messages = _conversation.PollMessages(1);

        Empty(messages);
    }

    [Fact]
    public void ZeroPolling_HasNoMessages()
    {
        var messages = _conversation.PollMessages(0);

        Empty(messages);
    }

    [Fact]
    public void NegativePolling_HasNoMessages()
    {
        var messages = _conversation.PollMessages(-1);

        Empty(messages);
    }

    [Fact]
    public void ChatClient_CanReceiveAMessage()
    {
        var greeting = new Message {Timestamp = DateTime.Now, Content = "Hi!"};

        _conversation.ReceiveMessage(greeting);

        var messages = _conversation.PollMessages(1);

        Single(messages);
        Contains(greeting, messages);
    }
}