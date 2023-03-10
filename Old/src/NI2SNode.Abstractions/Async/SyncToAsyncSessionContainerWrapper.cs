using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Async
{
    public class SyncToAsyncSessionContainerWrapper : IAsyncSessionContainer
    {
        readonly ISessionContainer _syncSessionContainer;

        public ISessionContainer SessionContainer
        {
            get { return _syncSessionContainer; }
        }

        public SyncToAsyncSessionContainerWrapper(ISessionContainer syncSessionContainer)
        {
            _syncSessionContainer = syncSessionContainer;
        }

        public ValueTask<ISession> GetSessionByIDAsync(string sessionID)
        {
            return new ValueTask<ISession>(_syncSessionContainer.GetSessionByID(sessionID));
        }

        public ValueTask<int> GetSessionCountAsync()
        {
            return new ValueTask<int>(_syncSessionContainer.GetSessionCount());
        }

        public ValueTask<IEnumerable<ISession>> GetSessionsAsync(Predicate<ISession>? criteria = null)
        {
            return new ValueTask<IEnumerable<ISession>>(_syncSessionContainer.GetSessions(criteria));
        }

        public ValueTask<IEnumerable<TAppSession>> GetSessionsAsync<TAppSession>(Predicate<TAppSession>? criteria = null) where TAppSession : ISession
        {
            return new ValueTask<IEnumerable<TAppSession>>(_syncSessionContainer.GetSessions<TAppSession>(criteria));
        }
    }
}