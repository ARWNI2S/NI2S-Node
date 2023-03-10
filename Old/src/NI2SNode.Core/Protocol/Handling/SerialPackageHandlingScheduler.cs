using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public class SerialPackageHandlingScheduler<TPackageInfo> : PackageHandlingSchedulerBase<TPackageInfo>
    {
        public override async ValueTask HandlePackage(ISession session, TPackageInfo package)
        {
            await HandlePackageInternal(session, package);
        }
    }
}