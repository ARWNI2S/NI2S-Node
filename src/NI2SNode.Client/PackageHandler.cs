namespace NI2S.Node.Client
{
    public delegate ValueTask PackageHandler<TReceivePackage>(EasyClient<TReceivePackage> sender, TReceivePackage package)
        where TReceivePackage : class;
}
