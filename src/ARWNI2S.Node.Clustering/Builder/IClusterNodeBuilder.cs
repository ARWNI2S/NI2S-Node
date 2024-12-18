namespace ARWNI2S.Clustering.Builder
{
    public interface IClusterNodeBuilder
    {
        IServiceProvider ServiceProvider { get; }

        IEngineBuilder CreateEngineBuilder();

    }
}
