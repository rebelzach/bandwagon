using Bandwagon.Web.Models;
using F23.StringSimilarity;

namespace Bandwagon.Web.Services
{
    public class GroupedMessageCollector
    {
        private readonly MessageCorrelator _messageCorrelator;


        List<StoredChatMessage> _recentMessages = new();
        public List<StoredChatMessage> RecentMessages => _recentMessages;


        List<MessageGroup> _groups = new();
        public List<MessageGroup> Groups => _groups;

        List<MessageGroup> _oldGroups = new();
        public List<MessageGroup> OldGroups => _oldGroups;

        public event EventHandler? GroupsUpdated;

        long? _lastRecievedTs;

        public List<StoredChatMessage> ChatLogWithoutGroups { get; private set; } = new();
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
            _groups.Clear();
            _oldGroups.Clear();
        }

        public void ReceiveMessage(StoredChatMessage newMessage)
        {
            if (newMessage.Message.Contains("hite", StringComparison.OrdinalIgnoreCase))
                return;
            _lastRecievedTs = newMessage.TmiSentTs;
            var windowStart = _lastRecievedTs - ConsiderMessageForGroupingWindowSeconds * 1000;

            _recentMessages = _recentMessages.Where(m => m.TmiSentTs > windowStart).ToList();

            var cooldownThresholdTime = newMessage.TmiSentTs - GroupCooldownSeconds * 1000;

            foreach (var group in _groups.ToList())
            {
                if (group.LastMessageAddedAt < cooldownThresholdTime)
                {
                    _oldGroups.Add(group);
                    _groups.Remove(group);
                    continue;
                }
            }

            // Check to see if the message fits into any of the current groups.
            foreach (var group in _groups.ToList())
            {
                var updatedGroup = _messageCorrelator.TryCorrelateWithGroup(
                    newMessage,
                    group,
                    MessageDistanceThreshold);

                if (updatedGroup is not null)
                {
                    _groups[_groups.IndexOf(group)] = updatedGroup;
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

                _groups.Add(newGroup);
                return;
            }

            _recentMessages.Add(newMessage);
        }
    }

    public record MessageGroup(StoredChatMessage RepresentativeMessage, List<ScoredMessage> Messages, long FirstMessageAt, long LastMessageAddedAt)
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public record ScoredMessage(StoredChatMessage Message, double? Score);
}
