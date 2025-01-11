using ARWNI2S.Cluster.Builder;
using ARWNI2S.Engine.Builder;

namespace ARWNI2S.Cluster.Extensibility
{
    internal static class EngineBuilderNodeModuleExtensions
    {

        internal static IEngineBuilder UseNodeModules(this IEngineBuilder engine, IClusterNodeBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(engine);
            ArgumentNullException.ThrowIfNull(builder);


            return engine;
        }
    }
}
