using System;
using System.Threading.Tasks;
using NI2S.Node;
using NI2S.Node.Protocol.Channel;

namespace SuperSocket.Server
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