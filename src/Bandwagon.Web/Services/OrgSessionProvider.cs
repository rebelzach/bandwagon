using System.Collections.Concurrent;

namespace Bandwagon.Web.Services;

public class OrgSessionProvider
{
    private readonly ConcurrentDictionary<string, OrgSession> _sessions = new();
    private readonly IServiceProvider _provider;

    public OrgSessionProvider(
        IServiceProvider provider,
        UserSessionProvider userSessionProvider)
    {
        _provider = provider;
    }

    public OrgSession Get(string orgId)
    {
        if (_sessions.TryGetValue(orgId, out var session))
        {
            return session;
        }

        session = _provider.GetRequiredService<OrgSession>();

        if (_sessions.TryAdd(orgId, session))
        {
            return session;
        }

        // Thread battle lost, get actual session
        if (_sessions.TryGetValue(orgId, out session))
        {
            return session;
        }

        throw new InvalidOperationException("Couldnt't get session");
    }
}
