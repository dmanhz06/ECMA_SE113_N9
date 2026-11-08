using System.Collections.Concurrent;
using ECMA.Domain.Interfaces;

namespace ECMA.Web.Services;

public sealed class AuthSessionStore(IClock clock)
{
    private static readonly ConcurrentDictionary<string, Session> Sessions = new();
    private static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(30);

    public string Start()
    {
        var id = Guid.NewGuid().ToString("N");
        Sessions[id] = new Session(clock.UtcNow);
        return id;
    }

    public bool TryTouch(string? id)
    {
        if (id is null || !Sessions.TryGetValue(id, out var session)) return false;

        lock (session)
        {
            var observedNow = clock.UtcNow;
            var now = observedNow > session.LastActivity ? observedNow : session.LastActivity;
            if (now - session.LastActivity >= IdleTimeout)
            {
                Sessions.TryRemove(KeyValuePair.Create(id, session));
                return false;
            }

            session.LastActivity = now;
            return true;
        }
    }

    public void End(string? id)
    {
        if (id is not null) Sessions.TryRemove(id, out _);
    }

    private sealed class Session(DateTime lastActivity)
    {
        public DateTime LastActivity { get; set; } = lastActivity;
    }
}
