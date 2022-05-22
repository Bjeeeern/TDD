using System;
using System.Linq;
using Core;
using Xunit;
using Assert = Tests.Utilities.AssertExtentions;

namespace Tests.TestSuites.Production;

public class Conversation
{
    private readonly Core.Conversation _conversation;
    private readonly Message _prompt;
    private readonly Message _response;

    public Conversation()
    {
        _conversation = new Core.Conversation();
        _prompt = new Message
        {
            Timestamp = DateTime.Now,
            Content = "Hi!"
        };
        _response = new Message
        {
            Timestamp = _prompt.Timestamp + TimeSpan.FromMinutes(1),
            Content = "What's up?"
        };
    }

    [Fact]
    public void HasNoMessagesInTheBeginning()
    {
        var messages = _conversation.PollMessages(1);

        Assert.Empty(messages);
    }

    [Fact]
    public void CanReceiveAndPollAMessage()
    {
        _conversation.ReceiveMessage(_prompt);

        var messages = _conversation.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(_prompt, messages);
    }

    [Fact]
    public void CanPollZeroMessages()
    {
        _conversation.ReceiveMessage(_prompt);

        var messages = _conversation.PollMessages(0);

        Assert.Empty(messages);
    }

    [Fact]
    public void CanPollNegativeMessages()
    {
        _conversation.ReceiveMessage(_prompt);

        var messages = _conversation.PollMessages(-1);

        Assert.Empty(messages);
    }

    [Fact]
    public void CanReceiveTwoMessages()
    {
        _conversation.ReceiveMessage(_prompt);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(2);

        Assert.Length(2, messages);
        Assert.Contains(_prompt, messages);
        Assert.Contains(_response, messages);
    }

    [Fact]
    public void CanPollMessagesInChronologicalOrder()
    {
        _conversation.ReceiveMessage(_response);
        _conversation.ReceiveMessage(_prompt);

        var messages = _conversation.PollMessages(2);

        Assert.Equal(_prompt, messages.First());
        Assert.Equal(_response, messages.Skip(1).First());
    }

    [Fact]
    public void CanPollLessMessagesThanReceived()
    {
        _conversation.ReceiveMessage(_prompt);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(1);

        Assert.Single(messages);
        Assert.Contains(_prompt, messages);
    }

    [Fact]
    public void CanPollMoreMessagesThanReceived()
    {
        _conversation.ReceiveMessage(_prompt);
        _conversation.ReceiveMessage(_response);

        var messages = _conversation.PollMessages(3);

        Assert.Length(2, messages);
        Assert.Contains(_prompt, messages);
        Assert.Contains(_response, messages);
    }
}