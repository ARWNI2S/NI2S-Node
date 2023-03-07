using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Async
{
    public class AsyncToSyncSessionContainerWrapper : ISessionContainer
    {
        readonly IAsyncSessionContainer _asyncSessionContainer;

        public AsyncToSyncSessionContainerWrapper(IAsyncSessionContainer asyncSessionContainer)
        {
            _asyncSessionContainer = asyncSessionContainer;
        }

        public ISession GetSessionByID(string sessionID)
        {
            return _asyncSessionContainer.GetSessionByIDAsync(sessionID).Result;
        }

        public int GetSessionCount()
        {
            return _asyncSessionContainer.GetSessionCountAsync().Result;
        }

        public IEnumerable<ISession> GetSessions(Predicate<ISession>? criteria)
        {
            return _asyncSessionContainer.GetSessionsAsync(criteria).Result;
        }

        public IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession>? criteria) where TAppSession : ISession
        {
            return _asyncSessionContainer.GetSessionsAsync(criteria).Result;
        }
    }
}