namespace NI2S.Node.Protocol.Session
{
    class DefaultSessionFactory : ISessionFactory
    {
        public Type SessionType => typeof(Session);

        public ISession Create()
        {
            return new Session();
        }
    }
}