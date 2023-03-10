using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Middleware
{
    public abstract class MiddlewareBase : IMiddleware
    {
        public int Order { get; protected set; } = 0;

        public virtual void Start(INode server)
        {

        }

        public virtual void Shutdown(INode server)
        {

        }

        public virtual ValueTask<bool> RegisterSession(ISession session)
        {
            return new ValueTask<bool>(true);
        }

        public virtual ValueTask<bool> UnRegisterSession(ISession session)
        {
            return new ValueTask<bool>(true);
        }
    }
}