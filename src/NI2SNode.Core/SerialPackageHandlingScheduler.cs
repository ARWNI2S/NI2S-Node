using NI2S.Node;
using System.Threading.Tasks;

namespace SuperSocket.Server
{
    public class SerialPackageHandlingScheduler<TPackageInfo> : PackageHandlingSchedulerBase<TPackageInfo>
    {
        public override async ValueTask HandlePackage(IAppSession session, TPackageInfo package)
        {
            await HandlePackageInternal(session, package);
        }
    }
}