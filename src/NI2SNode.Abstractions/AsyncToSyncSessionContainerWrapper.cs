using System;
using System.Collections.Generic;

namespace NI2S.Node
{
    public class AsyncToSyncSessionContainerWrapper : ISessionContainer
    {
        readonly IAsyncSessionContainer _asyncSessionContainer;

        public AsyncToSyncSessionContainerWrapper(IAsyncSessionContainer asyncSessionContainer)
        {
            _asyncSessionContainer = asyncSessionContainer;
        }

        public IAppSession GetSessionByID(string sessionID)
        {
            return _asyncSessionContainer.GetSessionByIDAsync(sessionID).Result;
        }

        public int GetSessionCount()
        {
            return _asyncSessionContainer.GetSessionCountAsync().Result;
        }

        public IEnumerable<IAppSession> GetSessions(Predicate<IAppSession>? criteria)
        {
            return _asyncSessionContainer.GetSessionsAsync(criteria).Result;
        }

        public IEnumerable<TAppSession> GetSessions<TAppSession>(Predicate<TAppSession>? criteria) where TAppSession : IAppSession
        {
            return _asyncSessionContainer.GetSessionsAsync<TAppSession>(criteria).Result;
        }
    }
}