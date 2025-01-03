namespace ARWNI2S.Engine.Session
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
