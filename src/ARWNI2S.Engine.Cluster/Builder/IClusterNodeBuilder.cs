using ARWNI2S.Engine.Builder;
using ARWNI2S.Extensibility;

namespace ARWNI2S.Cluster.Builder
{
    /// <summary>
    /// Defines a contract for a cluster node builder in a NI2S engine. A cluster node builder specifies the
    /// network communication for a node.
    /// </summary>
    public interface IClusterNodeBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IEngineBuilder"/>.
        /// </summary>
        /// <returns>The new <see cref="IEngineBuilder"/>.</returns>
        IEngineBuilder CreateEngineBuilder();

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> used to resolve services for routes.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the module data sources configured in the builder.
        /// </summary>
        ICollection<IModuleDataSource> DataSources { get; }
    }
}