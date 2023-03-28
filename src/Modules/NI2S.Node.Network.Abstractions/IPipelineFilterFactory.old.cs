namespace NI2S.Node.Networking
{
    public interface IPipelineFilterFactory<TPackageInfo>
    {
        IPipelineFilter<TPackageInfo> Create(object client);
    }
}