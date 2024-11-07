using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Session;

namespace ARWNI2S.Runtime.Network.Session
{
    class DefaultSessionFactory : ISessionFactory
    {
        public Type SessionType => typeof(AppSession);

        public IAppSession Create()
        {
            return new AppSession();
        }
    }
}