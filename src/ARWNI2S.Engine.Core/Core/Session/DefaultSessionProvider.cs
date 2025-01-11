// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Session;

namespace ARWNI2S.Engine.Core.Session
{
    internal class DefaultSessionProvider : ISessionProvider
    {
        //private readonly ISessionFactory _sessionFactory;
        //public DefaultSessionProvider(ISessionFactory sessionFactory)
        //{
        //    _sessionFactory = sessionFactory;
        //}
        //public ISession CreateSession()
        //{
        //    return _sessionFactory.CreateSession();
        //}

        public INiisSession Session { get; set; } = default!;
    }
}
