using System.Collections.Concurrent;

namespace Bandwagon.Web.Services;

public class UserSessionProvider
{
    private readonly ConcurrentDictionary<string, UserSession> _sessions = new();
    private readonly IServiceProvider _provider;

    public UserSessionProvider(
        IServiceProvider provider,
        UserCl)
    {
        _provider = provider;
    }

    public UserSession Get(string userId)
    {
        if (_sessions.TryGetValue(userId, out var session))
        {
            return session;
        }

        session = _provider.GetRequiredService<UserSession>();

        if (_sessions.TryAdd(userId, session))
        {
            return session;
        }

        // Thread battle lost, get actual session
        if (_sessions.TryGetValue(userId, out session))
        {
            return session;
        }

        throw new InvalidOperationException("Couldnt't get session");
    }
}
