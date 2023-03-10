using NI2S.Node.Protocol.Session;

namespace NI2S.Node
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