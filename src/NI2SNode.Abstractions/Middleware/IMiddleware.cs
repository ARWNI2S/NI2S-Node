using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Middleware
{
    public interface IMiddleware
    {
        int Order { get; }

        void Start(INode server);

        void Shutdown(INode server);

        ValueTask<bool> RegisterSession(ISession session);

        ValueTask<bool> UnRegisterSession(ISession session);
    }
}