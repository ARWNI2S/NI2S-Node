using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Cluster.Builder
{
    internal class DefaultClusterNodeBuilder : IClusterNodeBuilder
    {
        public DefaultClusterNodeBuilder(IEngineBuilder engineBuilder)
        {
            EngineBuilder = engineBuilder ?? throw new ArgumentNullException(nameof(engineBuilder));
            DataSources = [];
        }

        private IEngineBuilder EngineBuilder { get; }

        public IServiceProvider ServiceProvider => EngineBuilder.EngineServices;

        public ICollection<IModuleDataSource> DataSources { get; }

        public IEngineBuilder CreateEngineBuilder() => EngineBuilder.New();
    }
}