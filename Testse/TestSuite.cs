using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests;

public class TestSuite
{
    private readonly ChatClient _client;

    public TestSuite()
    {
        _client = new ChatClient();
    }
    
    [Fact]
    public void NewlyCreatedChatClient_HasNoMessages()
    {
        var messages = _client.PollMessages(10);
        Assert.Empty(messages);
    }

    [Fact]
    public void ChatClient_CanReceiveAMessage()
    {
        var testGreeting = new Message { Timestamp = DateTime.Now, Content = "こんにちは"};
        
        _client.ReceiveMessage(testGreeting);
        
        var messages = _client.PollMessages(10);
        Assert.Single(messages);
        Assert.Contains(testGreeting, messages);
    }
    
    [Fact]
    public void ChatClient_CanReceiveTwoMessages()
    {
        var testGreeting = new Message { Timestamp = DateTime.Now, Content = "こんにちは"};
        var testResponse = new Message { Timestamp = testGreeting.Timestamp + TimeSpan.FromMinutes(2), Content = "こんにちは"};
        
        _client.ReceiveMessage(testGreeting);
        _client.ReceiveMessage(testResponse);
        
        var messages = _client.PollMessages(10);
        AssertX.Length(2, messages);
        Assert.Contains(testGreeting, messages);
        Assert.Contains(testResponse, messages);
    }

    [Fact]
    public void ChatClient_CanPollLessThanReceived()
    {
        var testGreeting = new Message { Timestamp = DateTime.Now, Content = "こんにちは"};
        var testResponse = new Message { Timestamp = testGreeting.Timestamp + TimeSpan.FromMinutes(2), Content = "こんにちは"};
        
        _client.ReceiveMessage(testGreeting);
        _client.ReceiveMessage(testResponse);
        
        var messages = _client.PollMessages(1);
        Assert.Single(messages);
    }

    [Fact]
    public void Messages_ArePolledInChronologicalOrder()
    {
        var testGreeting = new Message { Timestamp = DateTime.Now, Content = "こんにちは"};
        var testResponse = new Message { Timestamp = testGreeting.Timestamp + TimeSpan.FromMinutes(2), Content = "こんにちは"};
        
        _client.ReceiveMessage(testResponse);
        _client.ReceiveMessage(testGreeting);
        
        var messages = _client.PollMessages(10);
        Assert.Equal(testGreeting, messages.First());
        Assert.Equal(testResponse, messages.Skip(1).First());
    }
}

public static class AssertX
{
    public static void Length<T>(int expected, IEnumerable<T> enumerable)
    {
        Assert.Equal(expected, enumerable.Count());
    }
}

public class Message
{
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
}

public class ChatClient
{
    private SortedList<DateTime, Message> _messages = new ();
    public IEnumerable<Message> PollMessages(int limit)
    {
        return _messages.Select(x => x.Value).Take(limit).AsEnumerable();
    }

    public void ReceiveMessage(Message content)
    {
        _messages.Add(content.Timestamp, content);
    }
}