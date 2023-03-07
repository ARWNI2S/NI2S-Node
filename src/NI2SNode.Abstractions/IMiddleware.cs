
using System.Threading.Tasks;

namespace NI2S.Node
{
    public interface IMiddleware
    {
        int Order { get; }

        void Start(IServer server);

        void Shutdown(IServer server);

        ValueTask<bool> RegisterSession(IAppSession session);

        ValueTask<bool> UnRegisterSession(IAppSession session);
    }
}