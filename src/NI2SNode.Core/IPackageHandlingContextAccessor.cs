using NI2S.Node;

namespace SuperSocket.Server
{

    public interface IPackageHandlingContextAccessor<TPackageInfo>
    {
        PackageHandlingContext<IAppSession, TPackageInfo>? PackageHandlingContext { get; set; }
    }


}
