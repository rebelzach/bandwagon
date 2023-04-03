using System.Collections.Concurrent;

namespace Bandwagon.Web.Services;

public class LiveStreamProvider
{
    private readonly ConcurrentDictionary<string, OrgSession> _sessions = new();
    private readonly IServiceProvider _provider;
    private readonly OrgSessionProvider _orgSessionProvider;

    public LiveStreamProvider(
        IServiceProvider provider,
        OrgSessionProvider orgSessionProvider)
    {
        _provider = provider;
        this._orgSessionProvider = orgSessionProvider;
    }

    /// <summary>
    /// Provides a live stream, for now use the org ID to lookup the live stream.
    /// </summary>
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
