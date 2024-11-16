using ARWNI2S.Infrastructure.Engine.Builder;

namespace ARWNI2S.Node.Hosting.Engine
{
    /// <summary>
    /// Defines a contract for a realy builder in a node. A relay builder specifies the network
    /// handlig for a node engine.
    /// </summary>
    public interface INodeRelayBuilder
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
        /// Gets the endpoint data sources configured in the builder.
        /// </summary>
        ICollection<RelayDataSource> DataSources { get; }
    }
}