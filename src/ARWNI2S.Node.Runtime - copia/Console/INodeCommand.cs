using ARWNI2S.Engine.Network.Command;
using ARWNI2S.Engine.Network.Protocol;

namespace ARWNI2S.Runtime.Console
{
    public interface INodeCommand : IAsyncCommand<StringPackageInfo>
    {
    }
}