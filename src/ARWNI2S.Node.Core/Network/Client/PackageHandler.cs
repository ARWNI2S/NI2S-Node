namespace ARWNI2S.Node.Core.Network.Client
{
    public delegate ValueTask PackageHandler<TReceivePackage>(NodeClient<TReceivePackage> sender, TReceivePackage package)
        where TReceivePackage : class;
}