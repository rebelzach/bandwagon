using F23.StringSimilarity;
using F23.StringSimilarity.Interfaces;

namespace Bandwagon.Web.Services;

public class MessageCorrelator
{
    //var algorithm = new NGram(4);
    //var algorithm = new LongestCommonSubsequence();
    //var algorithm = new QGram(4);
    private readonly INormalizedStringDistance _algorithm = new MetricLCS();

    public MessageGroup? CorrelateNewMessage(
        CollectedMessage testMessage, List<CollectedMessage> toCompare, double distanceThreshold, int countThreshold)
    {
        var matches = toCompare
            .Where(m => Distance(testMessage, m) <= distanceThreshold)
            .ToList();

        // Counts must be distinct by username before grouping
        if (matches.DistinctBy(m => m.Message.Username).Count() < countThreshold)
            return null;

        // matches need to relate to each other too.
        foreach (var match in matches.ToList())
        {
            var allMatch = matches
                .Where(m => m != match)
                .Select(m => Distance(testMessage, m))
                .All(ms => ms <= distanceThreshold);

            if (!allMatch)
                matches.Remove(match);
        }

        matches.Add(testMessage);

        // Counts must be distinct by username
        if (matches.DistinctBy(m => m.Message.Username).Count() < countThreshold)
            return null;

        var group = new MessageGroup(matches,
            matches.Select(m => m.Message.TmiSentTs).Min(),
            matches.Select(m => m.Message.TmiSentTs).Max());

        matches.ForEach(m => m.Group = group);

        return group;
    }

    public bool TryCorrelateWithGroup(CollectedMessage testMessage, MessageGroup group, double distanceThreshold)
    {
        var allMatch = group.Messages.All(m => Distance(testMessage, m) <= distanceThreshold);
        if (allMatch)
        {
            testMessage.Group = group;
            group.Messages.Add(testMessage);
            group.LastMessageAddedAt = testMessage.Message.TmiSentTs;
            return true;
        }
        return false;
    }

    private double Distance(CollectedMessage messageA, CollectedMessage messageB)
    {
        return _algorithm.Distance(
            messageA.Message.Message.ToLowerInvariant(), messageB.Message.Message.ToLowerInvariant());
    }
}
