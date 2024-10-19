
using ARWNI2S.Node.Core.Network;

namespace ARWNI2S.Node.Core.Runtime
{
    public interface IRuntimeContext
    {
        IServiceProvider ContextServices { get; }
        string LocalHost { get; }
        ConnectionInfo Connection { get; }
    }
}