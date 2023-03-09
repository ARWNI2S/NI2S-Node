using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public interface IPackageHandlingContextAccessor<TPackageInfo>
    {
        PackageHandlingContext<ISession, TPackageInfo>? PackageHandlingContext { get; set; }
    }
}
