using Bandwagon.Web.Models;

namespace Bandwagon.Web.Services;

public class GroupedMessageCollector
{
    private readonly object _messageLocker = new();
    private readonly object _groupLocker = new();

    private readonly MessageCorrelator _messageCorrelator;

    List<CollectedMessage> _recentMessages = new(5000);
    public List<CollectedMessage> RecentMessages
    {
        get
        {
            lock(_messageLocker)
            {
                return _recentMessages.TakeLast(100).ToList();
            }
        }
    }

    List<MessageGroup> _groups = new(500);
    public List<MessageGroup> Groups
    {
        get
        {
            lock(_groupLocker)
            {
                var groupCompareStart = _lastRecievedTs - GroupCooldownSeconds * 1000;
                return _groups
                    .Where(g => g.LastMessageAddedAt > groupCompareStart)
                    .OrderByDescending(g => g.Messages.Count)
                    .Take(LimitGroupCount)
                    .OrderBy(g => g.FirstMessageAt)
                    .ToList();
            }
        }
    }

    long? _lastRecievedTs;

    public double ConsiderMessageForGroupingWindowSeconds { get; set; } = 10.0;
    public double GroupCooldownSeconds { get; set; } = 10.0;
    public int MinimumRelatedMessagesToCreateNewGroup { get; set; } = 3;
    public double MessageDistanceThreshold { get; set; } = 0.5;
    public int LimitGroupCount { get; set; } = 3;

    public GroupedMessageCollector(MessageCorrelator messageCorrelator)
    {
        _messageCorrelator = messageCorrelator;
    }

    public void Reset()
    {
        lock(_messageLocker)
        {
            _recentMessages.Clear();
        }
        lock(_groupLocker)
        {
            _groups.Clear();
        }
    }

    public void ReceiveMessage(IChatMessage newMessage)
    {
        _lastRecievedTs = newMessage.TmiSentTs;

        var message = new CollectedMessage(newMessage);

        if (newMessage.Username == "Nightbot" || newMessage.Message.Trim().StartsWith("!"))
        {
            _recentMessages.Add(message);
            return;
        }

        // Check to see if the message fits into any of the current groups.
        var groupCompareStart = newMessage.TmiSentTs - GroupCooldownSeconds * 1000;
        lock (_groupLocker)
        {
            var groupsForComparison = _groups.Where(g => g.LastMessageAddedAt > groupCompareStart).ToList();
            foreach (var group in groupsForComparison)
            {
                var didCorrelate = _messageCorrelator.TryCorrelateWithGroup(
                    message,
                    group,
                    MessageDistanceThreshold);

                if (didCorrelate)
                {
                    _recentMessages.Add(message);
                    return;
                }
            }
        }

        var compareWindowStart = _lastRecievedTs - ConsiderMessageForGroupingWindowSeconds * 1000;
        List<CollectedMessage> msgsForComparison;
        lock (_messageLocker)
        {
            msgsForComparison = _recentMessages
                .Where(m => m.Group is null)
                .Where(m => m.Message.TmiSentTs > compareWindowStart)
                .ToList();
        }

        // Otherwise check to see if this message might start a new group.
        var newGroup = _messageCorrelator.CorrelateNewMessage(
            message,
            msgsForComparison,
            MessageDistanceThreshold,
            MinimumRelatedMessagesToCreateNewGroup);
        if (newGroup is not null)
        {
            lock(_groupLocker)
                _groups.Add(newGroup);
        }

        _recentMessages.Add(message);
    }
}

public class MessageGroup
{
    public List<CollectedMessage> Messages { get; set; }
    public long FirstMessageAt { get; set; }
    public long LastMessageAddedAt { get; set; }

    public MessageGroup(List<CollectedMessage> messages, long firstMessageAt, long lastMessageAddedAt)
    {
        Messages = messages;
        FirstMessageAt = firstMessageAt;
        LastMessageAddedAt = lastMessageAddedAt;
    }
}

public class CollectedMessage
{
    public IChatMessage Message { get; set; }
    public MessageGroup? Group { get; set; }

    public CollectedMessage(IChatMessage message)
    {
        Message = message;
    }
}

