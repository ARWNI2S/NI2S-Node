namespace ARWNI2S.Infrastructure.Engine.Builder
{
    public interface IEngineBuilder
    {
        IServiceProvider EngineServices { get; set; }

        IFeatureCollection EngineFeatures { get; }

        IDictionary<string, object> Properties { get; }

        UpdateDelegate Build();
        IEngineBuilder New();

        IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> frameProcessor);
    }
}
