namespace ARWNI2S.Infrastructure.Network.Protocol
{
    public interface IPipelineFilterFactory<TPackageInfo>
    {
        IPipelineFilter<TPackageInfo> Create(object client);
    }

    public interface IPipelineFilterFactory
    {
        IPipelineFilter Create(object client);
    }
}