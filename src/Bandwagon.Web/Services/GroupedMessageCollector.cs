using Bandwagon.Web.Models;

namespace Bandwagon.Web.Services;

public class GroupedMessageCollector
{
    private readonly MessageCorrelator _messageCorrelator;

    List<IChatMessage> _recentMessages = new();
    public List<IChatMessage> RecentMessages => _recentMessages;

    List<MessageGroup> _groups = new();
    public List<MessageGroup> Groups => _groups;

    List<MessageGroup> _oldGroups = new();
    public List<MessageGroup> OldGroups => _oldGroups;

    long? _lastRecievedTs;

    public List<IChatMessage> ChatLogWithoutGroups { get; private set; } = new();
    public double ConsiderMessageForGroupingWindowSeconds { get; set; } = 10.0;
    public double GroupCooldownSeconds { get; set; } = 10.0;
    public int MinimumRelatedMessagesToCreateNewGroup { get; set; } = 3;
    public double MessageDistanceThreshold { get; set; } = 0.5;

    public GroupedMessageCollector(MessageCorrelator messageCorrelator)
    {
        _messageCorrelator = messageCorrelator;
    }

    public void Reset()
    {
        _recentMessages.Clear();
        ReplaceGroups(new());
        _oldGroups.Clear();
    }

    public void ReceiveMessage(IChatMessage newMessage)
    {
        _lastRecievedTs = newMessage.TmiSentTs;
        var windowStart = _lastRecievedTs - ConsiderMessageForGroupingWindowSeconds * 1000;

        _recentMessages = _recentMessages.Where(m => m.TmiSentTs > windowStart).ToList();

        var cooldownThresholdTime = newMessage.TmiSentTs - GroupCooldownSeconds * 1000;

        var newGroups = _groups.ToList();

        foreach (var group in newGroups.ToList())
        {
            if (group.LastMessageAddedAt < cooldownThresholdTime)
            {
                _oldGroups.Add(group);
                newGroups.Remove(group);
                continue;
            }
        }

        // Check to see if the message fits into any of the current groups.
        foreach (var group in newGroups.ToList())
        {
            var updatedGroup = _messageCorrelator.TryCorrelateWithGroup(
                newMessage,
                group,
                MessageDistanceThreshold);

            if (updatedGroup is not null)
            {
                newGroups[newGroups.IndexOf(group)] = updatedGroup;
                ReplaceGroups(newGroups);
                return;
            }
        }

        // Otherwise check to see if this message might start a new group.
        var newGroup = _messageCorrelator.CorrelateNewMessage(
            newMessage,
            _recentMessages,
            MessageDistanceThreshold,
            MinimumRelatedMessagesToCreateNewGroup);
        if (newGroup is not null)
        {
            foreach (var m in newGroup.Messages.Select(m => m.Message))
                _recentMessages.Remove(m);

            newGroups.Add(newGroup);
            ReplaceGroups(newGroups);
            return;
        }

        _recentMessages.Add(newMessage);
    }

    private void ReplaceGroups(List<MessageGroup> groups)
    {
        Interlocked.Exchange(ref _groups, groups);
    }
}

public record MessageGroup(IChatMessage RepresentativeMessage, List<ScoredMessage> Messages, long FirstMessageAt, long LastMessageAddedAt)
{
    public Guid Id { get; } = Guid.NewGuid();
}

public record ScoredMessage(IChatMessage Message, double? Score);
