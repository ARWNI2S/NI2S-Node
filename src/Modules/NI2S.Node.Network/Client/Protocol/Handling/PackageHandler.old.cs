using System.Threading.Tasks;

namespace NI2S.Node.Client
{
    public delegate ValueTask PackageHandler<TReceivePackage>(NodeClient<TReceivePackage> sender, TReceivePackage package)
        where TReceivePackage : class;
}
