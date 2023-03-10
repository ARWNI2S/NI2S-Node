using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public interface ISessionContainer
    {
        ISession GetSessionByID(string sessionID);

        int GetSessionCount();

        IEnumerable<ISession> GetSessions(Predicate<ISession>? criteria = null);

        IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession>? criteria = null)
            where TAppSession : ISession;
    }

    namespace Async
    {
        public interface IAsyncSessionContainer
        {
            ValueTask<ISession> GetSessionByIDAsync(string sessionID);

            ValueTask<int> GetSessionCountAsync();

            ValueTask<IEnumerable<ISession>> GetSessionsAsync(Predicate<ISession>? criteria = null);

            ValueTask<IEnumerable<TAppSession>> GetSessionsAsync<TAppSession>(Predicate<TAppSession>? criteria = null)
                where TAppSession : ISession;
        }
    }
}