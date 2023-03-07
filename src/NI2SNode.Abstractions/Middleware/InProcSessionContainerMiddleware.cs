using System.Collections.Concurrent;

namespace NI2S.Node.Protocol.Session
{
    public class InProcSessionContainerMiddleware : MiddlewareBase, ISessionContainer
    {
        private ConcurrentDictionary<string, ISession> _sessions;

        public InProcSessionContainerMiddleware(IServiceProvider serviceProvider)
        {
            Order = int.MaxValue; // make sure it is the last middleware
            _sessions = new ConcurrentDictionary<string, ISession>(StringComparer.OrdinalIgnoreCase);
        }

        public override ValueTask<bool> RegisterSession(ISession session)
        {
            if (session is IHandshakeRequiredSession handshakeSession)
            {
                if (!handshakeSession.Handshaked)
                    return new ValueTask<bool>(true);
            }

            _sessions.TryAdd(session.SessionID, session);
            return new ValueTask<bool>(true);
        }

        public override ValueTask<bool> UnRegisterSession(ISession session)
        {
            _sessions.TryRemove(session.SessionID, out _);
            return new ValueTask<bool>(true);
        }

        public ISession GetSessionByID(string sessionID)
        {
            _sessions.TryGetValue(sessionID, out ISession? session);
            return session!;
        }

        public int GetSessionCount()
        {
            return _sessions.Count;
        }

        public IEnumerable<ISession> GetSessions(Predicate<ISession>? criteria = null)
        {
            var enumerator = _sessions.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var s = enumerator.Current.Value;

                if (s.State != SessionState.Connected)
                    continue;

                if (criteria == null || criteria(s))
                    yield return s;
            }
        }

        public IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession>? criteria = null) where TAppSession : ISession
        {
            var enumerator = _sessions.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value is TAppSession s)
                {
                    if (s.State != SessionState.Connected)
                        continue;

                    if (criteria == null || criteria(s))
                        yield return s;
                }
            }
        }
    }
}
