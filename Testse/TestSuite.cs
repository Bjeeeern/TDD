using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests;

public class TestSuite
{
    [Fact]
    public void NewlyCreatedChatClient_HasNoMessages()
    {
        var client = new ChatClient();

        var messages = client.PollMessages(10);
        Assert.Empty(messages);
    }

    [Fact]
    public void ChatClient_CanReceiveAMessage()
    {
        var client = new ChatClient();

        var content = "こんにちは";
        client.ReceiveMessage(content);
        
        var messages = client.PollMessages(10);
        Assert.Single(messages);
        Assert.Contains(content, messages);
    }
    
    [Fact]
    public void ChatClient_CanReceiveTwoMessages()
    {
        var client = new ChatClient();

        var content = "こんにちは";
        client.ReceiveMessage(content);
        client.ReceiveMessage(content);
        
        var messages = client.PollMessages(10);
        AssertX.Length(2, messages);
        Assert.Contains(content, messages);
    }

    [Fact]
    public void ChatClient_CanPollLessThanReceived()
    {
        var client = new ChatClient();

        var content = "こんにちは";
        client.ReceiveMessage(content);
        client.ReceiveMessage(content);
        
        var messages = client.PollMessages(1);
        Assert.Single(messages);
        Assert.Contains(content, messages);
    }
}

public static class AssertX
{
    public static void Length<T>(int expected, IEnumerable<T> enumerable)
    {
        Assert.Equal(expected, enumerable.Count());
    }
}

public class ChatClient
{
    private ICollection<string> _messages = new List<string>();
    public IEnumerable<string> PollMessages(int limit)
    {
        return _messages.Take(limit);
    }

    public void ReceiveMessage(string content)
    {
        _messages.Add(content);
    }
}