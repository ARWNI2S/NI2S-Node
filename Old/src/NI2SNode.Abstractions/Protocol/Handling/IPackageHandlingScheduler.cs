using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public interface IPackageHandlingScheduler<TPackageInfo>
    {
        void Initialize(IPackageHandler<TPackageInfo> packageHandler, Func<ISession, PackageHandlingException<TPackageInfo>, ValueTask<bool>> errorHandler);

        ValueTask HandlePackage(ISession session, TPackageInfo package);
    }
}