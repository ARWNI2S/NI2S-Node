using NI2S.Node;
using NI2S.Node.Protocol.Session;
using System.Threading.Tasks;

namespace NI2S.Node.Server
{
    public class ConcurrentPackageHandlingScheduler<TPackageInfo> : PackageHandlingSchedulerBase<TPackageInfo>
    {
        public override ValueTask HandlePackage(ISession session, TPackageInfo package)
        {
            HandlePackageInternal(session, package).DoNotAwait();
            return new ValueTask();
        }
    }
}