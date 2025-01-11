using ARWNI2S.Extensibility;

namespace ARWNI2S.Engine.Builder
{
    public interface IEngineBuilder
    {
        IServiceProvider EngineServices { get; set; }
        IModuleCollection NodeModules { get; }
        IDictionary<string, object> Properties { get; }

        INiisEngine Build();
        IEngineBuilder New();
    }
}
