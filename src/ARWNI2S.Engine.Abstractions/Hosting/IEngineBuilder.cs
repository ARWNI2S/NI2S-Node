
using ARWNI2S.Engine.Extensibility;

namespace ARWNI2S.Engine.Hosting
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
