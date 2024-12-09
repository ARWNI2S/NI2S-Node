namespace ARWNI2S.Engine.Builder
{
    public interface IEngineBuilder
    {
        IServiceProvider EngineServices { get; set; }
        IDictionary<string, object> Properties { get; }
        IFeatureCollection NodeFeatures { get; }

        INiisEngine Build();
        IEngineBuilder New();

        IEngineBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware);
    }
}
