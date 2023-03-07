using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public interface IPackageHandler<TReceivePackageInfo>
    {
        ValueTask Handle(ISession session, TReceivePackageInfo package);
    }
}