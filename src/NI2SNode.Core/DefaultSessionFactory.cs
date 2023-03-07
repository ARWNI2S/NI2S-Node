using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Server
{
    class DefaultSessionFactory : ISessionFactory
    {
        public Type SessionType => typeof(AppSession);

        public ISession Create()
        {
            return new AppSession();
        }
    }
}