using System.Threading.Tasks;

namespace NI2S.Node
{
    public interface IPackageHandler<TReceivePackageInfo>
    {
        ValueTask Handle(IAppSession session, TReceivePackageInfo package);
    }
}