using Bandwagon.Web.Models;
using F23.StringSimilarity;
using F23.StringSimilarity.Interfaces;

namespace Bandwagon.Web.Services;

public class MessageCorrelator
{
    // Most relevant: has the most occurrences of the lowest distance. (can probably just add the distances?)
    // Group, all the messages relate to all the other messages

    //var algorithm = new NGram(4);
    private INormalizedStringDistance _algorithm = new MetricLCS();
    //var algorithm = new LongestCommonSubsequence();
    //var algorithm = new QGram(4);

    public MessageGroup? CorrelateNewMessage(
        IChatMessage testMessage, List<IChatMessage> messages, double distanceThreshold, int countThreshold)
    {
        var matches = messages
            .Select(m => new ScoredMessage(m, _algorithm.Distance(testMessage.Message.ToLowerInvariant(), m.Message.ToLowerInvariant())))
            .Where(ms => ms.Score <= distanceThreshold)
            .ToList();
        // matches need to relate to each other too.
        foreach (var match in matches.ToList())
        {
            var allMatch = matches
                .Where(m => m != match)
                .Select(m => m.Message)
                .Select(m => new ScoredMessage(m, _algorithm.Distance(testMessage.Message.ToLowerInvariant(), m.Message.ToLowerInvariant())))
                .All(ms => ms.Score <= distanceThreshold);

            if (!allMatch)
                matches.Remove(match);
        }
        matches = matches.Append(new(testMessage, 0)).ToList();
        if (matches.Count >= countThreshold)
        {
            return new(matches.First().Message, matches,
                matches.Select(m => m.Message.TmiSentTs).Min(),
                matches.Select(m => m.Message.TmiSentTs).Max());
        }
        return null;
    }

    public MessageGroup? TryCorrelateWithGroup(IChatMessage testMessage, MessageGroup group, double distanceThreshold)
    {
        var allMatch = group.Messages
            .Select(m => m.Message)
            .Select(m => new ScoredMessage(m, _algorithm.Distance(testMessage.Message.ToLowerInvariant(), m.Message.ToLowerInvariant())))
            .All(ms => ms.Score <= distanceThreshold);
        if (allMatch)
        {
            return group with { Messages = group.Messages.Append(new(testMessage, 0)).ToList(), LastMessageAddedAt = testMessage.TmiSentTs };
        }
        return null;
    }
}
