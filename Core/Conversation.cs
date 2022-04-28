namespace Core;

public class Conversation
{
    private readonly SortedList<DateTime, Message> _messages = new();

    public IEnumerable<Message> PollMessages(int limit)
    {
        return _messages
            .Select(x => x.Value)
            .Take(limit)
            .AsEnumerable();
    }

    public void ReceiveMessage(Message content)
    {
        _messages.Add(content.Timestamp, content);
    }
}