using ARWNI2S.Engine.Network;
using ARWNI2S.Engine.Network.Session;

namespace ARWNI2S.Runtime.Network
{
    class DefaultSessionFactory : ISessionFactory
    {
        public Type SessionType => typeof(NodeSession);

        public INodeSession Create()
        {
            return new NodeSession();
        }
    }
}